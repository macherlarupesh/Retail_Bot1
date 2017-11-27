namespace MultiDialogsBot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using System.Data;
    using System.Configuration;
    using Models;
    using System.Data.SqlClient;


    [Serializable]
    public class RootDialog : IDialog<object>
    {

        private const string YesOption = "Yes";

        private const string NoOption = "No";

        //string Name = "";

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            await context.PostAsync("Hai, How are you..?");
            context.Wait(MessageName);
        }

        private async Task MessageName(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;


            await context.PostAsync("May I know your name....");

            context.Wait(MessageNameNext1);
        }

        private async Task MessageNameNext1(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            await context.PostAsync($"Hi {activity.Text}, Welcome to our Petronas Retail shopping.");
            string name = activity.Text;
            string CS = ConfigurationManager.ConnectionStrings["RetailData"].ConnectionString;
            SqlConnection con = new SqlConnection("data source =.\\SQLEXPRESS; initial catalog = BotData; user id = sa; password = sa123;");
            SqlCommand cmd = new SqlCommand("select * from Retail_Data where Name='" + name + "'", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                string his = dr["Past_History"].ToString();
                // return our reply to the user
                if (name == activity.Text)
                {
                    await context.PostAsync("I think you bought " + his + " from my shop....");
                    await context.PostAsync("Please tell me your Account No..(Ex:5100651846)");
                    context.Wait(MessageRecommedItems);

                }
                //this.ShowOptions(context);

            }
            else
            {
                context.Call(new NoDialog(), this.ResumeAfterOptionDialog);
            }
        }

        private async Task MessageRecommedItems(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            ulong account_no = Convert.ToUInt64(activity.Text);
            string recomitems = "";
            string CS = ConfigurationManager.ConnectionStrings["RecomItems"].ConnectionString;
            SqlConnection con = new SqlConnection("data source =.\\SQLEXPRESS; initial catalog = BotData; user id = sa; password = sa123;");
            SqlCommand cmd = new SqlCommand("select * from Recom_Items where Account_No=" + account_no, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            //if (dr.HasRows)
            //{
            //dr.Read();
            DataTable dt = new DataTable();
            dt.Load(dr);
            int numRows = dt.Rows.Count;

            //dr.Read();
            //string his = dr["Past_History"].ToString();
            // return our reply to the user
            if (account_no == Convert.ToUInt64(activity.Text))
            {
                for (int i = 0; i < numRows; i++)
                {
                    recomitems = recomitems + " " + dt.Rows[i]["Recommended_Items"] + "\n\n";
                }
                await context.PostAsync("May be following product promotions interesting for you..");
                await context.PostAsync($"{recomitems}");
                await context.PostAsync("Are you looking for a specific product please enter the Number");
                //context.Wait(MessageRecommedItemsPrice);
                //this.ShowOptions(context);
                context.Call(new RecomDialog(), this.ResumeAfterOptionDialog);

            }


            //}
            else
            {
                context.Call(new NoDialog(), this.ResumeAfterOptionDialog);
            }
        }

        public virtual async Task showOptions(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message.Text.ToLower().Contains("help") || message.Text.ToLower().Contains("support") || message.Text.ToLower().Contains("problem"))
            {
                await context.Forward(new SupportDialog(), this.ResumeAfterSupportDialog, message, CancellationToken.None);
            }
            else
            {
                this.ShowOptions(context);
            }
        }

        private void ShowOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() { YesOption, NoOption }, "We have discounts in some products, are you intrested?", "Not a valid option", 3);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case YesOption:
                        context.Call(new FlightsDialog(), this.ResumeAfterOptionDialog);
                        break;

                    case NoOption:
                        context.Call(new NoDialog(), this.ResumeAfterOptionDialog);
                        break;
                }

            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");

                context.Wait(this.MessageReceivedAsync);
            }

        }


        private async Task ResumeAfterSupportDialog(IDialogContext context, IAwaitable<int> result)
        {
            var ticketNumber = await result;

            await context.PostAsync($"Thanks for contacting our support team. Your ticket number is {ticketNumber}.");
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Message}");
            }
            finally
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }
    }
}