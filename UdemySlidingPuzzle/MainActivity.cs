using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Views;
using System.Collections;
using System;

namespace UdemySlidingPuzzle
{
	[Activity(Label = "UdemySlidingPuzzle", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{

		#region vars
		Button resetButton;
		GridLayout mainLayout;

		int gameViewWidth;
		int tileWidth;

		ArrayList tilesArray;
		ArrayList coordsArray;

		#endregion

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			 SetContentView (Resource.Layout.Main);

			// Make a call to SetGameView
			SetGameView();
			MakeTiles();
			RandomizeTiles();
		}

		private void MakeTiles()
		{
			tileWidth = gameViewWidth / 4;

			tilesArray = new ArrayList();
			coordsArray = new ArrayList();

			int counter = 1;
			#region MyRegion
			for (int h = 0; h < 4; h++)
			{
				for (int v = 0; v < 4; v++)
				{
					TextView textTile = new TextView(this);

					GridLayout.Spec rowSpec = GridLayout.InvokeSpec(h);
					GridLayout.Spec colSpec = GridLayout.InvokeSpec(v);

					GridLayout.LayoutParams tileLayoutParams = new GridLayout.LayoutParams(rowSpec, colSpec);

					textTile.Text = counter.ToString();
					textTile.SetTextColor(Color.Black);
					textTile.TextSize = 40;
					textTile.Gravity = GravityFlags.Center;

					tileLayoutParams.Width = tileWidth - 10;
					tileLayoutParams.Height = tileWidth - 10;
					tileLayoutParams.SetMargins(5, 5, 5, 5);

					textTile.LayoutParameters = tileLayoutParams;
					textTile.SetBackgroundColor(Color.Green);

					Point thisLoc = new Point(v, h);
					coordsArray.Add(thisLoc);
					tilesArray.Add(textTile);

					mainLayout.AddView(textTile);
					counter++;
				}
			}
			#endregion


			mainLayout.RemoveView((TextView)tilesArray[15]);
			tilesArray.RemoveAt(15);
		}

		private void RandomizeTiles()
		{
			Random myRand = new Random();
			foreach (TextView any in tilesArray)
			{
				int randIndex = myRand.Next(0, coordsArray.Count);
				Point thisRandLoc = (Point)coordsArray[randIndex];

				GridLayout.Spec rowSpec = GridLayout.InvokeSpec(thisRandLoc.Y);
				GridLayout.Spec colSpec = GridLayout.InvokeSpec(thisRandLoc.X);

				GridLayout.LayoutParams randLayoutParam = new GridLayout.LayoutParams(rowSpec, colSpec);
			}
		}

		private void SetGameView()
		{
			resetButton = FindViewById<Button>(Resource.Id.resetButtonID);
			resetButton.Click += ResetButton_Click;

			mainLayout = FindViewById<GridLayout>(Resource.Id.gameGridLayoutID);
			gameViewWidth = Resources.DisplayMetrics.WidthPixels;

			mainLayout.ColumnCount = 4;
			mainLayout.RowCount = 4;

			mainLayout.LayoutParameters = new LinearLayout.LayoutParams(gameViewWidth, gameViewWidth);
			mainLayout.SetBackgroundColor(Color.Gray);

		}

		private void ResetButton_Click(object sender, System.EventArgs e)
		{
			throw new System.NotImplementedException();
		}
	}
}

