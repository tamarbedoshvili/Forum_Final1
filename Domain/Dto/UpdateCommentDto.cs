namespace Final.Dto
{
    public class UpdateCommentDto
    {
        public int Id { get; set; }
        public int PostID { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
    }
}
