//using Application.Helper;
//using DocumentFormat.OpenXml.Drawing.Charts;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using System.Net;
//using System.Security.Cryptography;
//using System.Text;
//using System.Web;

//namespace SBSHEWEBAPI
//{

//    [ApiController]
//    [Route("api/[controller]")]
//    public class GetWayController : ControllerBase
//    {
//        private const string _key = "rzp_live_TlgUog8AWvJ7VI";
//        private const string _secret = "sv5TflUdI8VO6PmqRvnjgFHZ";


//        private readonly IUnitOfWork _unitOfWork;
//        public GetWayController(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        [HttpPost("PaymentDoRazorPay")]

//        public async Task<IActionResult> PaymentDoRazorPay()
//        {
//            try
//            {
//                string uid = HttpContext.Session.GetString("User");
//                string oid = HttpContext.Session.GetString("Key");
//                string UserDetail = HttpContext.Session.GetString("UserD");

//                if (string.IsNullOrEmpty(oid))
//                {
//                    return Unauthorized(new { message = "Could not make this payment ! ", redirect = "login" });
//                }

//                // ✅ 'List' ki jagah 'data' naam rakha
//                var data = _unitOfWork.GetWayWorking
//                            .OrderListAll("Print", Convert.ToInt32(oid), Convert.ToInt32(uid));

//                if (data == null || data.Count == 0)
//                    return NotFound(new { message = "Order not found" });

//                // ✅ Ab koi conflict nahi
//                int tid = data[0].Id;
//                decimal amount = 0;
//                string order_id = data[0].Code;
//                int productId = data[0].ReferenceId;

//                var R_orderId = CreateOrder();

//                Dictionary<string, string> notes = new Dictionary<string, string>()
//        {
//            { "Order_id", order_id },
//            { "tid", Convert.ToString(tid) }
//        };

//                var response = new
//                {
//                    Uuserd = UserDetail,
//                    Order = data[0],
//                    ProductId = productId,
//                    key = _key,
//                    amount = amount,
//                    currency = "INR",
//                    R_orderId = R_orderId,
//                    ProfileName = data[0].Name,
//                    Mobile = data[0].MobileNo,
//                    ProfileEmail = data[0].Email,
//                    Notes = notes,
//                    Url = "http://localhost:18516/Admin/HomeEnroll/OrderList"
//                };

//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                HttpContext.Session.Remove("Key");
//                return StatusCode(500, new { message = "Please try again ! ", error = ex.Message });
//            }
//        }
//        // CreateOrder — clean version
//        public string CreateOrder(int tid, decimal amount, string order_id)
//        {
//            try
//            {
//                Dictionary<string, string> notes = new Dictionary<string, string>()
//        {
//            { "Order_id", order_id },
//            { "tid", Convert.ToString(tid) }
//        };

//                RazorpayClient client = new RazorpayClient(_key, _secret);

//                Dictionary<string, object> options = new Dictionary<string, object>();
//                options.Add("amount", amount);
//                options.Add("receipt", order_id);
//                options.Add("currency", "INR");
//                options.Add("notes", notes);

//                Order order = client.Order.Create(options);
//                var R_orderId = order.Attributes["id"].ToString();

//                var rsp = ado.InsgetwayPrm(tid, R_orderId);

//                return R_orderId;
//            }
//            catch (Exception)
//            {
//                return null;
//            }
//        }
//        [HttpPost("Afterpayment")]
//        public async Task<IActionResult> Afterpayment([FromBody] AfterPaymentRequest req)
//        {
//            try
//            {
//                string uid = HttpContext.Session.GetString("User");
//                string oid = HttpContext.Session.GetString("Key");
//                string UserDetail = HttpContext.Session.GetString("UserD");

//                if (string.IsNullOrEmpty(oid))
//                {
//                    return Unauthorized(new { message = "Are you fool ! ", redirect = "Index" });
//                }

//                var List = ado.OrderListAll("Print", Convert.ToInt32(oid), Convert.ToInt32(uid));

//                if (List == null || List.Count == 0)
//                    return NotFound(new { message = "Order not found" });

//                string order_id = List[0].OrderId;
//                decimal amount = List[0].Amount;
//                string productId = Convert.ToString(List[0].ProductId);

//                // ✅ Request.Form ki jagah — [FromBody] se aayega
//                var R_orderId = req.R_orderId;
//                var paymentId = req.paymentId;
//                var signature = req.signature;
//                var paymentstatus = req.paymentstatus;

//                if (paymentstatus == "Fail")
//                {
//                    return Ok(new
//                    {
//                        success = false,
//                        status = "Fail",
//                        message = "Payment failed"
//                    });
//                }

//                var checkSignature = CompareSignature(R_orderId, paymentId, signature);

//                if (checkSignature)
//                {
//                    var response = ado.InsertOrderStstus(
//                                        order_id, paymentId, paymentstatus,
//                                        R_orderId, amount, Convert.ToInt32(oid), "");

//                    // ✅ Session set / remove — same
//                    HttpContext.Session.SetString("KeyOrdtbId", Convert.ToString(List[0].OId));
//                    HttpContext.Session.Remove("Key");

//                    string msg = "Your OrderId is " + order_id +
//                                 " and ReferenceNo Is " + paymentId +
//                                 " and Status Is " + paymentstatus;

//                    // ✅ RedirectToAction ki jagah — response me batao
//                    if (productId == "20411")
//                    {
//                        return Ok(new
//                        {
//                            success = true,
//                            status = "Success",
//                            message = msg,
//                            redirect = "StudentPaymentPrint",
//                            OId = List[0].OId
//                        });
//                    }

//                    return Ok(new
//                    {
//                        success = true,
//                        status = "Success",
//                        message = msg,
//                        productId = productId,
//                        OId = List[0].OId
//                    });
//                }
//                else
//                {
//                    HttpContext.Session.Remove("KeyOrdtbId");
//                    HttpContext.Session.Remove("Key");

//                    return Ok(new
//                    {
//                        success = false,
//                        status = "Fail",
//                        message = "Signature mismatch"
//                    });
//                }
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { message = "Please try again ! ", error = ex.Message });
//            }
//        }
//        private bool CompareSignature(string R_orderId, string paymentId, string signature)
//        {
//            var text = R_orderId + "|" + paymentId;
//            var secret = _secret;
//            var genratedSignature = Calcutate_hmac_sha256(text, secret);
//            if (genratedSignature == signature)
//            {
//                return true;
//            }
//            else
//                return false;
//        }

//        private string Calcutate_hmac_sha256(string text, string screte)
//        {
//            string result = "";
//            var enc = Encoding.Default;
//            byte[]
//            baText2beHashed = enc.GetBytes(text),
//            baSalt = enc.GetBytes(screte);
//            System.Security.Cryptography.HMACSHA256 hasher = new HMACSHA256(baSalt);
//            byte[] bahasedtext = hasher.ComputeHash(baText2beHashed);
//            result = string.Join("", bahasedtext.ToList().Select(b => b.ToString("x2")).ToArray());
//            return result;
//        }

//        [HttpPost("PaymentDoAtomIdbiEnr")]
//        public async Task<IActionResult> PaymentDoAtomIdbiEnr()
//        {
//            try
//            {
//                // ✅ Session wahi
//                string uid = HttpContext.Session.GetString("User");
//                string oid = HttpContext.Session.GetString("Key");
//                string UserDetail = HttpContext.Session.GetString("UserD");

//                string udf = ado1.getGetudf(Convert.ToInt32(UserDetail));

//                // ✅ oid null check — redirect ki jagah Unauthorized
//                if (string.IsNullOrEmpty(oid))
//                {
//                    string msg = "Are you fool ! ";
//                    return Unauthorized(new { message = msg, redirect = "Index" });
//                }

//                var List = ado.OrderListAll("Print", Convert.ToInt32(oid), Convert.ToInt32(uid));

//                if (List == null || List.Count == 0)
//                    return NotFound(new { message = "Order not found" });

//                // ✅ Same variables
//                string tid = Convert.ToString(List[0].OId);
//                decimal amount = List[0].Amount;
//                string order_id = List[0].OrderId;
//                string productid = Convert.ToString(List[0].ProductId);

//                string payInstrument = "";

//                // ============ Atom PayRequest Build (SAME) ============
//                Payrequest.RootObject rt = new Payrequest.RootObject();
//                Payrequest.MsgBdy mb = new Payrequest.MsgBdy();
//                Payrequest.HeadDetails hd = new Payrequest.HeadDetails();
//                Payrequest.MerchDetails md = new Payrequest.MerchDetails();
//                Payrequest.PayDetails pd = new Payrequest.PayDetails();
//                Payrequest.CustDetails cd = new Payrequest.CustDetails();
//                Payrequest.Extras ex = new Payrequest.Extras();
//                Payrequest.Payrequest pr = new Payrequest.Payrequest();

//                hd.version = "OTSv1.1";
//                hd.api = "AUTH";
//                hd.platform = "FLASH";

//                md.merchId = "580646";
//                md.userId = "580646";
//                md.password = "ae30de43";
//                md.merchTxnDate = List[0].ODate;
//                md.merchTxnId = List[0].OrderId;

//                pd.amount = Convert.ToString(amount);
//                pd.product = "ASSOCIATION";
//                pd.custAccNo = "213232323";
//                pd.txnCurrency = "INR";

//                if (List[0].EmailId == "")
//                    cd.custEmail = "yourmail@mail.com";
//                else
//                    cd.custEmail = List[0].EmailId;

//                if (List[0].EmailId == "")
//                    cd.custMobile = "0000000000";
//                else
//                    cd.custMobile = List[0].MobileNo;

//                ex.udf1 = uid;
//                ex.udf2 = UserDetail;
//                ex.udf3 = tid;
//                ex.udf4 = oid;
//                ex.udf5 = productid;
//                ex.udf21 = "8299~ST JOHN AMBULANCE INTERNATIONAL ASSOCIATION~stjohncouncil.co.in";

//                pr.headDetails = hd;
//                pr.merchDetails = md;
//                pr.payDetails = pd;
//                pr.custDetails = cd;
//                pr.extras = ex;

//                rt.payInstrument = pr;
//                var json = new Nancy.Json.JavaScriptSerializer().Serialize(rt);

//                // ============ Encryption (SAME) ============
//                string passphrase = "42E79687D3259314B66FF6CCC37C46E4";
//                string salt = "42E79687D3259314B66FF6CCC37C46E4";

//                byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
//                int iterations = 65536;
//                int keysize = 256;
//                string hashAlgorithm = "SHA1";
//                string Encryptval = Encrypt(json, passphrase, salt, iv, iterations);

//                // ============ HTTP Call (SAME) ============
//                string testurleq = "https://payment1.atomtech.in/ots/aipay/auth?merchId=580646&encData=" + Encryptval;

//                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(testurleq);
//                ServicePointManager.Expect100Continue = true;
//                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
//                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

//                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
//                Encoding encoding = new UTF8Encoding();
//                byte[] data = encoding.GetBytes(json);
//                request.ProtocolVersion = HttpVersion.Version11;
//                request.Method = "POST";
//                request.ContentType = "application/json";
//                request.ContentLength = data.Length;

//                Stream stream = request.GetRequestStream();
//                stream.Write(data, 0, data.Length);
//                stream.Close();

//                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
//                string jsonresponse = response.ToString();

//                StreamReader reader = new StreamReader(response.GetResponseStream());
//                string temp = null;
//                while ((temp = reader.ReadLine()) != null)
//                {
//                    jsonresponse += temp;
//                }

//                var result = jsonresponse.Replace("System.Net.HttpWebResponse", "");
//                var uri = new Uri("http://atom.in?" + result);
//                var query = HttpUtility.ParseQueryString(uri.Query);
//                string encData = query.Get("encData");

//                // ============ Decryption (SAME) ============
//                string passphrase1 = "1C63298535D99FA02406746704584BDF";
//                string salt1 = "1C63298535D99FA02406746704584BDF";

//                string Decryptval = decrypt(encData, passphrase1, salt1, iv, iterations);

//                Payverify.Payverify objectres = new Payverify.Payverify();
//                objectres = JsonConvert.DeserializeObject<Payverify.Payverify>(Decryptval);

//                string txnMessage = objectres.responseDetails.txnMessage;
//                Tok_id = objectres.atomTokenId;

//                // ✅ ViewBag ki jagah — response DTO
//                return Ok(new
//                {
//                    Uuserd = udf,
//                    Tok_id = Tok_id,
//                    custMobile = cd.custMobile,
//                    custEmail = cd.custEmail,
//                    txnMessage = txnMessage
//                });
//            }
//            catch (Exception ex)
//            {
//                string UserDetail = HttpContext.Session.GetString("UserD");

//                string udf = ado1.getGetudf(Convert.ToInt32(UserDetail));

//                return StatusCode(500, new
//                {
//                    Uuserd = udf,
//                    message = "Please try again ! ",
//                    error = ex.Message
//                });
//            }
//        }

//        public String Encrypt(String plainText, String passphrase, String salt, Byte[] iv, int iterations)
//        {
//            var plainBytes = Encoding.UTF8.GetBytes(plainText);
//            string data = ByteArrayToHexString(Encrypt(plainBytes, GetSymmetricAlgorithm(passphrase, salt, iv, iterations))).ToUpper();


//            return data;
//        }

//        public String decrypt(String plainText, String passphrase, String salt, Byte[] iv, int iterations)
//        {
//            byte[] str = HexStringToByte(plainText);

//            string data1 = Encoding.UTF8.GetString(decrypt(str, GetSymmetricAlgorithm(passphrase, salt, iv, iterations)));
//            return data1;
//        }
//        public byte[] Encrypt(byte[] plainBytes, SymmetricAlgorithm sa)
//        {
//            return sa.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);

//        }
//        public byte[] decrypt(byte[] plainBytes, SymmetricAlgorithm sa)
//        {
//            return sa.CreateDecryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
//        }
//        public SymmetricAlgorithm GetSymmetricAlgorithm(String passphrase, String salt, Byte[] iv, int iterations)
//        {
//            var saltBytes = new byte[16];
//            var ivBytes = new byte[16];
//            Rfc2898DeriveBytes rfcdb = new System.Security.Cryptography.Rfc2898DeriveBytes(passphrase, Encoding.UTF8.GetBytes(salt), iterations, HashAlgorithmName.SHA512);
//            saltBytes = rfcdb.GetBytes(32);
//            var tempBytes = iv;
//            Array.Copy(tempBytes, ivBytes, Math.Min(ivBytes.Length, tempBytes.Length));
//            var rij = new RijndaelManaged(); //SymmetricAlgorithm.Create();
//            rij.Mode = CipherMode.CBC;
//            rij.Padding = PaddingMode.PKCS7;
//            rij.FeedbackSize = 128;
//            rij.KeySize = 128;

//            rij.BlockSize = 128;
//            rij.Key = saltBytes;
//            rij.IV = ivBytes;
//            return rij;
//        }
//        protected static byte[] HexStringToByte(string hexString)
//        {
//            try
//            {
//                int bytesCount = (hexString.Length) / 2;
//                byte[] bytes = new byte[bytesCount];
//                for (int x = 0; x < bytesCount; ++x)
//                {
//                    bytes[x] = Convert.ToByte(hexString.Substring(x * 2, 2), 16);
//                }
//                return bytes;
//            }
//            catch
//            {
//                throw;
//            }
//        }
//        public static string ByteArrayToHexString(byte[] ba)
//        {
//            StringBuilder hex = new StringBuilder(ba.Length * 2);
//            foreach (byte b in ba)
//                hex.AppendFormat("{0:x2}", b);
//            return hex.ToString();
//        }


//    }
//}
