using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;

namespace Phoneword {

	[Activity(Label = "Phoneword", MainLauncher = true, Icon = "@mipmap/icon")]

	public class MainActivity : Activity {

		//Private vars:
		private string translatedNumber;
		private EditText phoneNumberText;
		private Button callButton;
		private Button translateButton;

		//Handler for translate button:
		private void translateButtonClickHandler(object Sender, EventArgs e) {

			// Translate user's alphanumeric phone number to numeric
			translatedNumber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);
			if (String.IsNullOrWhiteSpace(translatedNumber))
			{
				callButton.Text = "Call";
				callButton.Enabled = false;
			}
			else
			{
				callButton.Text = "Call " + translatedNumber;
				callButton.Enabled = true;
			}
		}

		//Handler for call button:
		private void callButtonClickHandler(object Sender, EventArgs e) {
			
			// On "Call" button click, try to dial phone number.
			var callDialog = new AlertDialog.Builder(this);
			callDialog.SetMessage("Call " + translatedNumber + "?");
			callDialog.SetNeutralButton("Call", delegate
			{
					// Create intent to dial phone
					var callIntent = new Intent(Intent.ActionCall);
				callIntent.SetData(Android.Net.Uri.Parse("tel:" + translatedNumber));
				StartActivity(callIntent);
			});
			callDialog.SetNegativeButton("Cancel", delegate { });

			// Show the alert dialog to the user and wait for response.
			callDialog.Show();
		}

        //OnCreate override:
		protected override void OnCreate(Bundle savedInstanceState) {
			
			//Call super 'OnCreate'
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

            //Init our vars.
            translatedNumber = string.Empty;
            phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
		    translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
	  	    callButton = FindViewById<Button>(Resource.Id.CallButton);

			//Disable the "Call" button
			callButton.Enabled = false;

            //Set handlers for button clicks.
			translateButton.Click += translateButtonClickHandler;


			callButton.Click += callButtonClickHandler;
		}

	}
}

