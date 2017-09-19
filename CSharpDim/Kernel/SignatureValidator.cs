using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using CSharpDim.Messages;
using CSharpDim.Util;

namespace CSharpDim.Kernel
{
	public class SignatureValidator
	{
		private readonly HMAC _signatureGenerator;

		private readonly Encoding _encoder;

		/// <param name="key">Shared key used to initialize the digest.</param>
		/// <param name="algorithm"></param>
		public SignatureValidator(string key, string algorithm) {
			_encoder = new UTF8Encoding();

			_signatureGenerator = HMAC.Create(algorithm);
			_signatureGenerator.Key = _encoder.GetBytes(key);
		}

		#region ISignatureValidator implementation

		/// <summary>
		/// Creates the signature.
		/// </summary>
		/// <returns>The signature.</returns>
		/// <param name="message">Message.</param>
		public string CreateSignature(Message message) {
			_signatureGenerator.Initialize();

			var messages = GetMessagesToAddForDigest(message);

			// For all items update the signature
			foreach (var item in messages) {
				var sourceBytes = _encoder.GetBytes(item);
				_signatureGenerator.TransformBlock(sourceBytes, 0, sourceBytes.Length, null, 0);
			}

			_signatureGenerator.TransformFinalBlock(new byte[0], 0, 0);

			// Calculate the digest and remove -
			return BitConverter.ToString(_signatureGenerator.Hash).Replace("-", "").ToLower();
		}

		/// <summary>
		/// Determines whether this instance is valid signature the specified message.
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="message">Message.</param>
		public bool IsValidSignature(Message message) {
			var calculatedSignature = CreateSignature(message);
			Log.Info($"Expected Signature: {calculatedSignature}");
			return string.Equals(message.HMac, calculatedSignature, StringComparison.OrdinalIgnoreCase);
		}

		#endregion

		/// <summary>
		/// Gets the messages to add for digest.
		/// </summary>
		/// <returns>The messages to add for digest.</returns>
		/// <param name="message">Message.</param>
		private static IEnumerable<string> GetMessagesToAddForDigest(Message message) {
			if (message == null) {
				return new List<string>();
			}

			return new List<string> {
				JsonSerializer.Serialize(message.Header),
				JsonSerializer.Serialize(message.ParentHeader),
				JsonSerializer.Serialize(message.Metadata),
				message.Content
			};
		}
	}
}