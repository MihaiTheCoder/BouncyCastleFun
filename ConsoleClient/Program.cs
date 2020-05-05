using BouncyCastleFun;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using System;
using System.IO;
using System.Text;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var caName = new X509Name("CN=TestCA");
            var eeName = new X509Name("CN=TestEE");
            AsymmetricCipherKeyPair caKey = Class1.GenerateEcKeyPair("secp256r1");
            AsymmetricCipherKeyPair eeKey = Class1.GenerateRsaKeyPair(2048);

            X509Certificate caCert = Class1.GenerateCertificate(caName, caName, caKey.Private, caKey.Public);
            X509Certificate eeCert = Class1.GenerateCertificate(caName, eeName, caKey.Private, eeKey.Public);

            bool caOk = Class1.ValidateSelfSignedCert(caCert, caKey.Public);
            var eeOk = Class1.ValidateSelfSignedCert(eeCert, caKey.Public);



            string x = "El ciapa murata";
            byte[] content = Encoding.ASCII.GetBytes(x);
            var signature = Class1.Sign(caCert.SigAlgName, caKey.Private, content);
            bool validationResult = Class1.ValidateSignature(caCert.SigAlgName, caCert.GetPublicKey(), content, signature);

            var pkcs = new Pkcs12Store(File.Open("path.pfx", FileMode.Open), "password".ToCharArray());
            

        }
    }
}
