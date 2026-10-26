using Application.DTOs;
using Application.Helper;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;

namespace SBSHEWEBAPI
{

    [ApiController]
    [Route("api/[controller]")]
    public class GetWayController : ControllerBase
    {
        private const string _key = "rzp_live_TlgUog8AWvJ7VI";
        private const string _secret = "sv5TflUdI8VO6PmqRvnjgFHZ";


        private readonly IUnitOfWork _unitOfWork;
        public GetWayController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("paymentDo-razorPay")]

        public async Task<IActionResult> PaymentDoRazorPay([FromBody] AmountDetailRequest request)
        {
            try
            {
                // ✅ Validation
                if (request == null || request.UserId <= 0 || request.OId <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "userId and oId are required"
                    });
                }




                // ✅ 'List' ki jagah 'data' naam rakha
                var data = _unitOfWork.GetWayWorking.OrderListAll("Print", request.OId, request.UserId);

                if (data == null || data.Count == 0)
                    return NotFound(new { message = "Order not found" });

                // ✅ Ab koi conflict nahi
                int tid = data[0].OId;
                decimal amount = data[0].Amount * 100;   // ✅ Paise mein (Razorpay format)
                string order_id = data[0].OrderId;
                int productId = data[0].ProductId;
                int classNo = Convert.ToInt32(data[0].ClassNo);

                var R_orderId = CreateOrder(request.UserId, request.OId);

                // ✅ Agar CreateOrder fail hua toh debug info ke saath error
                if (string.IsNullOrEmpty(R_orderId))
                {
                    return StatusCode(500, new
                    {
                        success = false,
                        message = "Razorpay order creation failed",
                        debug = new
                        {
                            userId = request.UserId,
                            oId = request.OId,
                            amount = amount,
                            key = _key,
                            dataCount = data.Count
                        }
                    });
                }

                Dictionary<string, string> notes = new Dictionary<string, string>()
        {
            { "Order_id", order_id },
            { "tid", Convert.ToString(tid) }
        };

                var response = new
                {
                    // Uuserd = UserDetail,
                    Order = data[0],
                    ProductId = productId,
                    key = _key,
                    amount = amount,
                    currency = "INR",
                    R_orderId = R_orderId,
                    ProfileName = data[0].Name,
                    Mobile = data[0].MobileNo,
                    ProfileEmail = data[0].EmailId,
                    Notes = notes
                    //Url = "http://localhost:18516/Admin/HomeEnroll/OrderList"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                HttpContext.Session.Remove("Key");
                return StatusCode(500, new { message = "Please try again ! ", error = ex.Message });
            }
        }


        // CreateOrder — clean version
        private string CreateOrder(int userId, int oId)
        {
            try
            {


                var data = _unitOfWork.GetWayWorking.OrderListAll("Print", Convert.ToInt32(oId), Convert.ToInt32(userId));
                int tid = data[0].OId;
                decimal amount = data[0].Amount * 100;
                string order_id = data[0].OrderId;

                Dictionary<string, string> notes = new Dictionary<string, string>()
        {
            { "Order_id", order_id },
            { "tid", Convert.ToString(tid) }
        };

                RazorpayClient client = new RazorpayClient(_key, _secret);

                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", amount);
                options.Add("receipt", order_id);
                options.Add("currency", "INR");
                options.Add("notes", notes);

                Order order = client.Order.Create(options);
                var R_orderId = order.Attributes["id"].ToString();

                var rsp = _unitOfWork.GetWayWorking.InsgetwayPrm(tid, R_orderId);

                return R_orderId;
            }
            catch (Exception)
            {
                return null;
            }
        }


        [HttpPost("after-payment")]
        public async Task<IActionResult> Afterpayment([FromForm] AfterPaymentRequest request)
        {
            try
            {
                if (request == null || request.UserId <= 0 || request.OId <= 0)
                    return Unauthorized(new { success = false, message = "Invalid request" });

                var List = _unitOfWork.GetWayWorking
                            .OrderListAll("Print", request.OId, request.UserId);

                if (List == null || List.Count == 0)
                    return NotFound(new { success = false, message = "Order not found" });

                string order_id = List[0].OrderId;
                decimal amount = List[0].Amount;
                string productId = Convert.ToString(List[0].ProductId);

                // ✅ Form values (case match frontend se)
                var R_orderId = Request.Form["R_orderId"].ToString();
                var paymentId = Request.Form["PaymentId"].ToString();
                var signature = Request.Form["Signature"].ToString();
                var paymentstatus = Request.Form["PaymentStatus"].ToString();

                if (paymentstatus == "Fail")
                {
                    return Ok(new
                    {
                        success = false,
                        status = "Fail",
                        message = "Payment failed"
                    });
                }

                var checkSignature = CompareSignature(R_orderId, paymentId, signature);

                if (checkSignature)
                {
                    var response = _unitOfWork.GetWayWorking.InsertOrderStstus(
                                        order_id, paymentId, paymentstatus,
                                        R_orderId, amount, request.OId, "");

                    string msg = "Your OrderId is " + order_id +
                                 " and ReferenceNo Is " + paymentId +
                                 " and Status Is " + paymentstatus;

                    if (productId == "4")
                    {
                        string Insertmsg = await _unitOfWork.Wallet.CreateOnline(
                            request.UserId,
                            Convert.ToDecimal(amount));
                    }

                    return Ok(new
                    {
                        success = true,
                        status = "Success",
                        message = msg,
                        productId = productId,
                        OId = List[0].OId
                    });
                }
                else
                {
                    return Ok(new
                    {
                        success = false,
                        status = "Fail",
                        message = "Signature mismatch"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Please try again!",
                    error = ex.Message
                });
            }
        }

        private bool CompareSignature(string R_orderId, string paymentId, string signature)
        {
            var text = R_orderId + "|" + paymentId;
            var secret = _secret;
            var genratedSignature = Calcutate_hmac_sha256(text, secret);
            if (genratedSignature == signature)
            {
                return true;
            }
            else
                return false;
        }

        private string Calcutate_hmac_sha256(string text, string screte)
        {
            string result = "";
            var enc = Encoding.Default;
            byte[]
            baText2beHashed = enc.GetBytes(text),
            baSalt = enc.GetBytes(screte);
            System.Security.Cryptography.HMACSHA256 hasher = new HMACSHA256(baSalt);
            byte[] bahasedtext = hasher.ComputeHash(baText2beHashed);
            result = string.Join("", bahasedtext.ToList().Select(b => b.ToString("x2")).ToArray());
            return result;
        }



        private String Encrypt(String plainText, String passphrase, String salt, Byte[] iv, int iterations)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            string data = ByteArrayToHexString(Encrypt(plainBytes, GetSymmetricAlgorithm(passphrase, salt, iv, iterations))).ToUpper();


            return data;
        }

        private String decrypt(String plainText, String passphrase, String salt, Byte[] iv, int iterations)
        {
            byte[] str = HexStringToByte(plainText);

            string data1 = Encoding.UTF8.GetString(decrypt(str, GetSymmetricAlgorithm(passphrase, salt, iv, iterations)));
            return data1;
        }
        private byte[] Encrypt(byte[] plainBytes, SymmetricAlgorithm sa)
        {
            return sa.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }
        private byte[] decrypt(byte[] plainBytes, SymmetricAlgorithm sa)
        {
            return sa.CreateDecryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }
        private SymmetricAlgorithm GetSymmetricAlgorithm(String passphrase, String salt, Byte[] iv, int iterations)
        {
            var saltBytes = new byte[16];
            var ivBytes = new byte[16];
            Rfc2898DeriveBytes rfcdb = new System.Security.Cryptography.Rfc2898DeriveBytes(passphrase, Encoding.UTF8.GetBytes(salt), iterations, HashAlgorithmName.SHA512);
            saltBytes = rfcdb.GetBytes(32);
            var tempBytes = iv;
            Array.Copy(tempBytes, ivBytes, Math.Min(ivBytes.Length, tempBytes.Length));
            var rij = new RijndaelManaged(); //SymmetricAlgorithm.Create();
            rij.Mode = CipherMode.CBC;
            rij.Padding = PaddingMode.PKCS7;
            rij.FeedbackSize = 128;
            rij.KeySize = 128;

            rij.BlockSize = 128;
            rij.Key = saltBytes;
            rij.IV = ivBytes;
            return rij;
        }
        protected static byte[] HexStringToByte(string hexString)
        {
            try
            {
                int bytesCount = (hexString.Length) / 2;
                byte[] bytes = new byte[bytesCount];
                for (int x = 0; x < bytesCount; ++x)
                {
                    bytes[x] = Convert.ToByte(hexString.Substring(x * 2, 2), 16);
                }
                return bytes;
            }
            catch
            {
                throw;
            }
        }
        private static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }



    }
}
