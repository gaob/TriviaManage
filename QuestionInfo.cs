﻿using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace TriviaManage
{
	public class QuestionInfo
	{
		/// <summary>
		/// The _mobile service client dynamically instanciated
		/// </summary>
		private MobileServiceHelper _mobileServiceClient;

		/// <summary>
		/// Initializes a new instance of the <see cref="QuestionInfo"/> class.
		/// </summary>
		public QuestionInfo()
		{
			// Get a reference to the mobile service client
			_mobileServiceClient = MobileServiceHelper.DefaultService;

			// Initialize the _QuestionItemTable with a default query that retrieves all questions.
			_questionItemTable = _mobileServiceClient.ServiceClient.GetTable<QuestionItem>();
		}

		/// <summary>
		/// A query linked to the QuestionItem table in the mobile service configured to return all items.
		/// </summary>
		private IMobileServiceTable<QuestionItem> _questionItemTable;

		/// <summary>
		/// Container to hold the question items from the mobile service
		/// </summary>
		private MobileServiceCollection<QuestionItem, QuestionItem> _questionItemList;

		/// <summary>
		/// Refreshes the questions.
		/// </summary>
		/// <returns>Task.</returns>
		public async Task RefreshQuestions()
		{
			QuestionList = await _questionItemTable.ToCollectionAsync();
		}

		/// <summary>
		/// Gets or sets the question list.
		/// </summary>
		/// <value>
		/// The question list.
		/// </value>
		public MobileServiceCollection<QuestionItem, QuestionItem> QuestionList
		{
			get { return _questionItemList; }
			set
			{
				_questionItemList = value;
			}
		}

		/// <summary>
		/// Saves or updates the the specified question item based on the CurrentUIMode
		/// </summary>
		/// <param name="QuestionItem">The question item.</param>
		/// <returns>Task.</returns>
		async internal Task Save(QuestionItem questionItem)
		{
			if (CurrentUIMode == UIModes.Adding)
			{
				await AddQuestion(questionItem);
			}else if (CurrentUIMode == UIModes.Editing)
			{
				await UpdateQuestion(questionItem);
			}
		}

		/// <summary>
		/// Gets or sets the current UI mode.
		/// </summary>
		/// <value>The current UI mode.</value>
		public UIModes CurrentUIMode { get; set; }

		/// <summary>
		/// Adds the question
		/// </summary>
		/// <returns>Task.</returns>
		async internal Task AddQuestion(QuestionItem questionItem)
		{
			await _questionItemTable.InsertAsync(questionItem);
		}

		/// <summary>
		/// Updates the question
		/// </summary>
		/// <returns>Task.</returns>
		async internal Task UpdateQuestion(QuestionItem questionItem)
		{
			await _questionItemTable.UpdateAsync(questionItem);
		}

		/// <summary>
		/// Deletes the question then refreshes the question list
		/// </summary>
		async internal Task DeleteQuestion(QuestionItem questionItem)
		{
			await _questionItemTable.DeleteAsync(questionItem);
			await RefreshQuestions();
		}
	}
}
