namespace BlogApp.Entity
{
    public class Follow
    {
        public int FollowerId { get; set; }
        public User Follower { get; set; }

        public int FolloweeId { get; set; }
        public User Followee { get; set; }
    }
}