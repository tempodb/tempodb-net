using Newtonsoft.Json;

namespace Client.Model
{
  /// <summary>
  ///  Represents a summary of the delete series operationd
  /// </summary>
  public class DeleteSummary
  {
    ///  <param name="deleted"> The number of deleted series</param>
    public DeleteSummary()
    {
      Deleted = 0;
    }

    [JsonProperty(PropertyName = "deleted")]
    public int Deleted { get; private set; }
  }
}
