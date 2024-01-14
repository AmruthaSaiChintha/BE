namespace AadharVerify.Models
{
    public class ImageProof
    {
        public int Id { get; set; }
        public byte[] FileContent { get; set; }

        public int UserId { get; set; }
        public Data User { get; set; }
    }
}
