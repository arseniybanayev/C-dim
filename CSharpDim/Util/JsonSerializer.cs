using Newtonsoft.Json;

namespace CSharpDim.Util
{
	public static class JsonSerializer
	{
		public static T Deserialize<T>(string value) {
			return JsonConvert.DeserializeObject<T>(value);
		}

		public static string Serialize<T>(T obj) {
			return JsonConvert.SerializeObject(obj);
		}
	}
}