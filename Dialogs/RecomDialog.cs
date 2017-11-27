namespace MultiDialogsBot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class RecomDialog : IDialog<object>
    {
        private const string YesOption = "Yes";

        private const string NoOption = "No";

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.SelectProduct);
        }

        public virtual async Task SelectProduct(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            int prodno = Convert.ToInt32(activity.Text);

            //await context.PostAsync("Yes");
            switch(prodno)
            {
                case 1:
                    await context.PostAsync("Ok, Cost of SL Pistachios 105g is Rs.150");
                    await context.PostAsync("Are you sure to purchase it...?(Yes or No)");
                    context.Wait(this.Confirmation123);
                    break;

                case 2:
                    await context.PostAsync("Ok, Cost of PILOT BALL PEN BLUE is Rs.75");
                    await context.PostAsync("Are you sure to purchase it...?(Yes or No)");
                    context.Wait(this.Confirmation123);
                    break;

                case 3:
                    await context.PostAsync("Ok, Cost of DELFI TOP X LARGE TR is Rs.120");
                    await context.PostAsync("Are you sure to purchase it...?(Yes or No)");
                    context.Wait(this.Confirmation123);
                    break;

                case 4:
                    await context.PostAsync("Ok, Cost of FOOD SERVICES-OTHERS is Rs.250");
                    await context.PostAsync("Are you sure to purchase it...?(Yes or No)");
                    context.Wait(this.Confirmation123);
                    break;

                case 5:
                    await context.PostAsync("Ok, Cost of RED BULL PRODUCT EUR is Rs.180");
                    await context.PostAsync("Are you sure to purchase it...?(Yes or No)");
                    context.Wait(this.Confirmation123);
                    break;

                case 6:
                    await context.PostAsync("Ok, Cost of NESTLE CRUNCHY BITE is Rs.175");
                    await context.PostAsync("Are you sure to purchase it...?(Yes or No)");
                    context.Wait(this.Confirmation123);
                    break;

                case 7:
                    await context.PostAsync("Ok, Cost of PEJOY CHOCOLATE 39GM is Rs.60");
                    await context.PostAsync("Are you sure to purchase it...?(Yes or No)");
                    context.Wait(this.Confirmation123);
                    break;

                case 8:
                    await context.PostAsync("Ok, Cost of SPRITZER HOT & WARM is Rs.135");
                    await context.PostAsync("Are you sure to purchase it...?(Yes or No)");
                    context.Wait(this.Confirmation123);
                    break;

                case 9:
                    await context.PostAsync("Ok, Cost of MAGNOLIA UHT CHOCOLA is Rs.50");
                    await context.PostAsync("Are you sure to purchase it...?(Yes or No)");
                    context.Wait(this.Confirmation123);
                    break;

                default:
                    await context.PostAsync("Please select the correct number...");
                    break;
            }
        }

        private async Task Confirmation123(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            if (activity.Text.ToLower().Contains("yes"))
            {
                await context.PostAsync("Ok, your order will be confirmed...our team will consult u for further details");
            }
            else if (activity.Text.ToLower().Contains("no"))
            {
                //await context.PostAsync("Thank you...visit again");
                //this.ShowOptions(context);
            }
            this.ShowOptions(context);
        }

        private async Task FinalDialog(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Thanks for purchasing...");
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

                context.Call(new RootDialog(), this.ResumeAfterOptionDialog);
            }

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
               context.Wait(this.SelectProduct);
            }
        }
    }
}