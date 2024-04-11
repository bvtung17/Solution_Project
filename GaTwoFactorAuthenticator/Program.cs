using Google.Authenticator;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        // Khởi tạo đối tượng TwoFactorAuthenticator
        TwoFactorAuthenticator authenticator = new TwoFactorAuthenticator();

        // Tạo secret key (đây là thông tin mật của bạn, nên được lưu trữ an toàn)
        var gaSecret = CommonHelper.RandomString(12);
        string secretKey = gaSecret.Encrypt("MY7JshJLGHVowuut9ZsU5emEW1KlqXquOfXOFbYoByBsVD55EDht1uF3ufNgq2Tb");

        // Thông tin ứng dụng và người dùng
        string appName = "MyApp";
        string userEmail = "user@example.com";

        // Tạo URL cho QR code
        var setupInfo = authenticator.GenerateSetupCode("user@example.comm", userEmail, CommonHelper.ConvertSecretToBytes(gaSecret, false), 300);
        var base64String = setupInfo.QrCodeSetupImageUrl.Replace("data:image/png;base64,", "");

        // Convert Base64 string to byte array
        byte[] imageBytes = Convert.FromBase64String(base64String);

        // Create an image from the byte array
        using (MemoryStream ms = new MemoryStream(imageBytes))
        {
            Image image = Image.FromStream(ms);

            // Save or use the image as required
            image.Save("output_method1.png", ImageFormat.Png);
        }

        Console.WriteLine("Scan the QR code with Google Authenticator:");
        Console.WriteLine(userEmail);

        // Bạn cần hiển thị QR code trong ứng dụng của mình. 
        // Đây chỉ là URL của QR code, bạn cần sử dụng một thư viện hoặc dịch vụ để tạo và hiển thị nó.
    }


}

public static class CommonHelper
{
    public static string Encrypt(this string plainText, string hashKey)
    {
        var initVector = hashKey.Substring(0, 16);

        if (string.IsNullOrEmpty(plainText))
            return string.Empty;

        var initVectorBytes = Encoding.UTF8.GetBytes(initVector);
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        var password = new PasswordDeriveBytes(hashKey, null);
        var keyBytes = password.GetBytes(256 / 8);
        var symmetricKey = new AesManaged
        {
            Padding = PaddingMode.Zeros,
            Mode = CipherMode.CBC
        };
        var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
        var memoryStream = new MemoryStream();
        var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        cryptoStream.FlushFinalBlock();
        var cipherTextBytes = memoryStream.ToArray();
        memoryStream.Close();
        cryptoStream.Close();
        return Convert.ToBase64String(cipherTextBytes);
    }
    public static byte[] ConvertSecretToBytes(string secret, bool secretIsBase32) =>
          secretIsBase32 ? Base32Encoding.ToBytes(secret) : Encoding.UTF8.GetBytes(secret);
    public static string RandomString(int length = 10)
    {
        var rand = new Random();
        const string pool = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var builder = new StringBuilder();

        for (var i = 0; i < length; i++)
        {
            var c = pool[rand.Next(0, pool.Length)];
            builder.Append(c);
        }

        return builder.ToString();
    }
}
