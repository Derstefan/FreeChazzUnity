using UnityEditor;
using UnityEngine;


    public class Util
    {
        public static PieceDTO getPieceDTOById(string pieceId, PieceDTO[] pDTOs)
        {
            foreach (PieceDTO pDTO in pDTOs)
            {
                if (pDTO != null && pDTO.pieceId == pieceId)
                {
                    return pDTO;
                }
            }
            return null;
        }

    public static string GenerateRandomString(int length)
    {
        const string charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        int charsetLength = charset.Length;

        string randomString = string.Empty;
        System.Random random = new System.Random();

        for (int i = 0; i < length; i++)
        {
            int randomIndex = random.Next(0, charsetLength);
            randomString += charset[randomIndex];
        }

        return randomString;
    }
}