namespace FlashTimes.Models;
public class Set
{   
    [Key]
    public int SetID { get; set; }
    public int UserID { get; set; }
    public string SetName { get; set; }
    public int SetLength { get; set; }

    public User User { get; set; }

    

}
