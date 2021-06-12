using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Proteo5.HL
{
    class RSAHL
    {
        public static void PersistNewAsymmetricKeyPair(int keySizeBits, string containerName)
        {
            // string key = "";
            try
            {
                CspParameters cspParams = new CspParameters();
                cspParams.KeyContainerName = containerName;
                cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
                RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(keySizeBits, cspParams)
                { PersistKeyInCsp = true };

            }
            catch (Exception ex)
            {
                throw ex;//  key = string.Empty;
            }
            //return key;
        }

        public static bool DeleteAsymmetricKeyPair(string containerName)
        {
            bool result = false;
            try
            {
                CspParameters cspParams = new CspParameters() { KeyContainerName = containerName };
                cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
                RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(cspParams) { PersistKeyInCsp = false };
                rsaProvider.Clear();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public static string GetFullRSAKey(string containerName)
        {
            string key = "";
            try
            {
                CspParameters cspParams = new() { KeyContainerName = containerName };
                cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
                RSACryptoServiceProvider rsaProvider = new(cspParams);
                key = RSAKeyExtensions.ToXmlString(rsaProvider, true);
            }
            catch (Exception ex)
            {
                key = string.Empty;
            }


            return key;
        }

        public static RSA GetFullRSA(string containerName)
        {

            try
            {
                CspParameters cspParams = new CspParameters() { KeyContainerName = containerName };
                cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
                RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(cspParams);
                return rsaProvider;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetPublicRSAKey(string containerName)
        {
            string key = "";
            try
            {
                CspParameters cspParams = new CspParameters() { KeyContainerName = containerName };
                cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
                RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(cspParams);
                key = RSAKeyExtensions.ToXmlString(rsaProvider, false);
            }
            catch (Exception ex)
            {
                key = string.Empty;
            }


            return key;
        }
          

        internal static bool TryKeyContainerPermissionCheck(string secretKeyName)
        {

            bool returnValue = false;

            WindowsIdentity current = WindowsIdentity.GetCurrent();

            WindowsPrincipal currentPrincipal = new WindowsPrincipal(current);

            if (currentPrincipal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                try
                {
                    foreach (string fileName in Directory.GetFiles(
                        @"C:\Documents and Settings\All Users\" +
                        @"Application Data\Microsoft\Crypto\RSA\MachineKeys"))
                    {
                        FileInfo fi = new FileInfo(fileName);

                        if (fi.Length <= 1024 * 5)
                        { // no key file should be greater then 5KB
                            using (StreamReader sr = fi.OpenText())
                            {
                                string fileData = sr.ReadToEnd();
                                if (fileData.Contains(secretKeyName))
                                { // this is our file

                                    FileSecurity fileSecurity = fi.GetAccessControl();

                                    bool currentIdentityFoundInACL = false;
                                    foreach (FileSystemAccessRule rule in fileSecurity
                                        .GetAccessRules(
                                            true,
                                            true,
                                            typeof(NTAccount)
                                        )
                                    )
                                    {
                                        if (rule.IdentityReference.Value.ToLower() ==
                                            current.Name.ToLower()
                                        )
                                        {
                                            returnValue = true;
                                            currentIdentityFoundInACL = true;
                                            break;
                                        }
                                    }

                                    if (!currentIdentityFoundInACL)
                                    {
                                        fileSecurity.AddAccessRule(
                                            new FileSystemAccessRule(
                                                current.Name,
                                                FileSystemRights.FullControl,
                                                AccessControlType.Allow
                                            )
                                        );

                                        fi.SetAccessControl(fileSecurity);

                                        returnValue = true;
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    throw;
                }
                catch { }
            }

            return returnValue;
        }
    }

    internal static class RSAKeyExtensions
    {
        #region JSON
        //internal static void FromJsonString(this RSA rsa, string jsonString)
        //{
        //    Check.Argument.IsNotEmpty(jsonString, nameof(jsonString));
        //    try
        //    {
        //        var paramsJson = JsonConvert.DeserializeObject<RSAParametersJson>(jsonString);

        //        RSAParameters parameters = new RSAParameters();

        //        parameters.Modulus = paramsJson.Modulus != null ? Convert.FromBase64String(paramsJson.Modulus) : null;
        //        parameters.Exponent = paramsJson.Exponent != null ? Convert.FromBase64String(paramsJson.Exponent) : null;
        //        parameters.P = paramsJson.P != null ? Convert.FromBase64String(paramsJson.P) : null;
        //        parameters.Q = paramsJson.Q != null ? Convert.FromBase64String(paramsJson.Q) : null;
        //        parameters.DP = paramsJson.DP != null ? Convert.FromBase64String(paramsJson.DP) : null;
        //        parameters.DQ = paramsJson.DQ != null ? Convert.FromBase64String(paramsJson.DQ) : null;
        //        parameters.InverseQ = paramsJson.InverseQ != null ? Convert.FromBase64String(paramsJson.InverseQ) : null;
        //        parameters.D = paramsJson.D != null ? Convert.FromBase64String(paramsJson.D) : null;
        //        rsa.ImportParameters(parameters);
        //    }
        //    catch
        //    {
        //        throw new Exception("Invalid JSON RSA key.");
        //    }
        //}

        //internal static string ToJsonString(this RSA rsa, bool includePrivateParameters)
        //{
        //    RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);

        //    var parasJson = new RSAParametersJson()
        //    {
        //        Modulus = parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
        //        Exponent = parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
        //        P = parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
        //        Q = parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
        //        DP = parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
        //        DQ = parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
        //        InverseQ = parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
        //        D = parameters.D != null ? Convert.ToBase64String(parameters.D) : null
        //    };

        //    return JsonConvert.SerializeObject(parasJson);
        //}
        #endregion

        #region XML

        public static void FromXmlString(RSA rsa, string xmlString)
        {
            RSAParameters parameters = new RSAParameters();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Exponent": parameters.Exponent = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "P": parameters.P = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Q": parameters.Q = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DP": parameters.DP = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DQ": parameters.DQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "InverseQ": parameters.InverseQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "D": parameters.D = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }

            rsa.ImportParameters(parameters);
        }

        public static string ToXmlString(RSA rsa, bool includePrivateParameters)
        {
            RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);

            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                  parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
                  parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
                  parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
                  parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
                  parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
                  parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
                  parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
                  parameters.D != null ? Convert.ToBase64String(parameters.D) : null);
        }

        #endregion
    }
}
