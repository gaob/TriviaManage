using System;
using Newtonsoft.Json;

namespace TriviaManage
{
	public class QuestionItem
	{
		private string _id;

		[JsonProperty(PropertyName = "id")]
		public string Id
		{
			get { return _id; }
			set
			{
				_id = value;
			}
		}

		private string _text;

		[JsonProperty(PropertyName = "questionText")]
		public string questionText
		{
			get { return _text; }
			set
			{
				_text = value;
			}
		}

		private string _one;

		[JsonProperty(PropertyName = "answerOne")]
		public string answerOne
		{
			get { return _one; }
			set
			{
				_one = value;
			}
		}

		private string _two;

		[JsonProperty(PropertyName = "answerTwo")]
		public string answerTwo
		{
			get { return _two; }
			set
			{
				_two = value;
			}
		}

		private string _three;

		[JsonProperty(PropertyName = "answerThree")]
		public string answerThree
		{
			get { return _three; }
			set
			{
				_three = value;
			}
		}

		private string _four;

		[JsonProperty(PropertyName = "answerFour")]
		public string answerFour
		{
			get { return _four; }
			set
			{
				_four = value;
			}
		}

		private string _identifier;

		[JsonProperty(PropertyName = "identifier")]
		public string identifier
		{
			get { return _identifier; }
			set
			{
				_identifier = value;
			}
		}
	}
}
