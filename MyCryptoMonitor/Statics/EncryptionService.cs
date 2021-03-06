﻿using MyCryptoMonitor.Forms;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace MyCryptoMonitor.Statics
{
    public static class EncryptionService
    {
        #region Private Fields

        private const string CHECKVALUE = "Success";
        private static string _password = string.Empty;

        #endregion Private Fields

        #region Public Methods

        public static byte[] AESDecryptBytes(byte[] cryptBytes, byte[] passBytes, byte[] saltBytes)
        {
            byte[] clearBytes = null;
            var key = new Rfc2898DeriveBytes(passBytes, saltBytes, 32768);

            using (Aes aes = new AesManaged())
            {
                aes.KeySize = 256;
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cryptBytes, 0, cryptBytes.Length);
                        cs.Close();
                    }
                    clearBytes = ms.ToArray();
                }
            }
            return clearBytes;
        }

        public static string AesDecryptString(string cryptText)
        {
            return AesDecryptString(cryptText, _password, UserConfigService.SaltKey);
        }

        public static string AesDecryptString(string cryptText, string passText)
        {
            return AesDecryptString(cryptText, passText, UserConfigService.SaltKey);
        }

        public static string AesDecryptString(string cryptText, string passText, string saltText)
        {
            try
            {
                byte[] cryptBytes = Convert.FromBase64String(cryptText);
                byte[] passBytes = Encoding.UTF8.GetBytes(passText);
                byte[] saltBytes = Encoding.UTF8.GetBytes(saltText);

                return Encoding.UTF8.GetString(AESDecryptBytes(cryptBytes, passBytes, saltBytes));
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static byte[] AESEncryptBytes(byte[] clearBytes, byte[] passBytes, byte[] saltBytes)
        {
            byte[] encryptedBytes = null;
            var key = new Rfc2898DeriveBytes(passBytes, saltBytes, 32768);

            using (Aes aes = new AesManaged())
            {
                aes.KeySize = 256;
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }
            return encryptedBytes;
        }

        public static string AesEncryptString(string clearText)
        {
            return AesEncryptString(clearText, _password, UserConfigService.SaltKey);
        }

        public static string AesEncryptString(string clearText, string passText)
        {
            return AesEncryptString(clearText, passText, UserConfigService.SaltKey);
        }

        public static string AesEncryptString(string clearText, string passText, string saltText)
        {
            try
            {
                byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
                byte[] passBytes = Encoding.UTF8.GetBytes(passText);
                byte[] saltBytes = Encoding.UTF8.GetBytes(saltText);

                return Convert.ToBase64String(AESEncryptBytes(clearBytes, passBytes, saltBytes));
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static void DecryptFiles()
        {
            PortfolioService.ToggleEncryption();
            UserConfigService.EncryptionCheck = string.Empty;
            UserConfigService.Encrypted = false;
            AlertService.Save();
        }

        public static void EncryptFiles(string password)
        {
            _password = password;

            PortfolioService.ToggleEncryption();
            UserConfigService.EncryptionCheck = AesEncryptString(CHECKVALUE);
            UserConfigService.Encrypted = true;
            AlertService.Save();
        }

        public static void Unlock()
        {
            using (FrmUnlock form = new FrmUnlock())
            {
                var result = form.ShowDialog();

                if (result == DialogResult.OK)
                {
                    if (!ValidatePassword(form.PasswordInput))
                    {
                        Unlock();
                        return;
                    }

                    _password = form.PasswordInput;
                }
                else if (result == DialogResult.Abort)
                {
                    if (MainService.ConfirmReset())
                        MainService.Reset();
                    else
                        Unlock();
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        public static bool ValidatePassword(string password)
        {
            return AesDecryptString(UserConfigService.EncryptionCheck, password).ExtEquals(CHECKVALUE);
        }

        #endregion Public Methods
    }
}