using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using CryptoGenesis.Models;
using System.Security.Cryptography;

namespace CryptoGenesis.Controllers
{
    public static class Helpers
    {
        public static string ComputeHash386(string pStr)
        {
            using(var shaComputer = SHA384.Create())
            {
                return Encoding.UTF8.GetString(shaComputer.ComputeHash(Encoding.UTF8.GetBytes(pStr)));
            }
        }

        public static StandartResponse ServerError(string pError = null)
        {
            return new StandartResponse
            {
                OperationSuccessfully = false,
                response = "Internal server error" + (string.IsNullOrEmpty(pError) ? "." : (": " + pError))
            };
        }

        public static StandartResponse StandarError(string pError)
        {
            return new StandartResponse
            {
                OperationSuccessfully = false,
                response = "Error: " + pError
            };
        }

        public static string GenerateByteArrayHex(int pLengthBytes)
        {
            Random rnd = new Random();

            byte[] byteArray = new byte[pLengthBytes];

            for(int i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = (byte)rnd.Next(0, 255);
            }
            return ByteArrayToString(byteArray);
        }


        public static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }
    }
}