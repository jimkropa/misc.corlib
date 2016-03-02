// TODO: Enable IOC of injection of SymmetricAlgorithm by non-static Encryptor Factory and Decryptor Factory
namespace MiscCorLib.Security.Cryptography
{
	using System.Security.Cryptography;

	public class SymmetricTransformerFactory
	{
		private readonly SymmetricAlgorithm algorithm;

		// For dependency injection:
		public SymmetricTransformerFactory(
			SymmetricAlgorithm algorithm)
		{
			this.algorithm = algorithm;
		}

		/*
		public Encryptor CreateEncryptor()
		{
			return new Encryptor(this.algorithm, );
		}
		*/
	}
}