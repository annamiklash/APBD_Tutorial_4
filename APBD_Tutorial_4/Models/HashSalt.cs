namespace APBD_Tutorial_4.Model
{
    public class HashSalt
    {
        public string Hash { get; set; }
        public string Salt { get; set; }

        public HashSalt(string hash, string salt)
        {
            Hash = hash;
            Salt = salt;
        }

    }
}