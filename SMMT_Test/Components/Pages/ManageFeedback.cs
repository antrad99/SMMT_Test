using Microsoft.AspNetCore.Components;
using SMMT_Test.Services;
using SMMT_Test.Dtos;
using Radzen;

namespace SMMT_Test.Components.Pages
{
    public partial class ManageFeedback
    {
        [Inject]
        private IFeedbackService feedbackService { get; set; }
        [Inject]
        private DialogService dialogService { get; set; }

        private List<FeedbackDto> feedbacks;
        DataGridGridLines GridLines = DataGridGridLines.Both;

        protected override async Task OnInitializedAsync()
        {
            feedbacks = await feedbackService.GetAllFeedback();
        }
        private async Task Delete(FeedbackDto feedback)
        {
            bool? isConfirmed = await dialogService.Confirm("Are you sure?", "Delete Feedback", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (isConfirmed == true)
            {
                await feedbackService.DeleteFeedback((int)feedback.Id);
                feedbacks = await feedbackService.GetAllFeedback();
            }
        }


        private async Task Create()
        {
            FeedbackDto model = await dialogService.OpenAsync<AddFeedback>("Add Feedback");


            if (model != null)
            {
                feedbacks = await feedbackService.GetAllFeedback(); 
                await dialogService.Alert("Record created successfully!");
            }
        }


        private async Task Edit(FeedbackDto args)
        {
            FeedbackDto model = await dialogService.OpenAsync<EditFeedback>("Edit Feedback", new Dictionary<string, object> { { "Feedback", args } });


            if (model != null)
            {
                feedbacks = await feedbackService.GetAllFeedback();
                await dialogService.Alert("Record updated successfully!");
            }
        }
    }
}
