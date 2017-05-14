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

		Point emptySpot;

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
					MyTextView textTile = new MyTextView(this);

					GridLayout.Spec rowSpec = GridLayout.InvokeSpec(h);
					GridLayout.Spec colSpec = GridLayout.InvokeSpec(v);

					GridLayout.LayoutParams tileLayoutParams = new GridLayout.LayoutParams(rowSpec, colSpec);

					//	 0 1 2 3 v/x
					// 0 T T T T
					// 1 T T T T
					// 2 T T T T
					// 3 T T T T

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

					textTile.xPos = thisLoc.X;
					textTile.yPos = thisLoc.Y;

					textTile.Touch += TextTile_Touch;

					tilesArray.Add(textTile);

					mainLayout.AddView(textTile);
					counter++;
				}
			}
			#endregion


			mainLayout.RemoveView((MyTextView)tilesArray[15]);
			tilesArray.RemoveAt(15);
		}

		// x=2, y=3
		// x=3, y=3

		private void TextTile_Touch(object sender, View.TouchEventArgs e)
		{
			if (e.Event.Action==MotionEventActions.Up)
			{
				MyTextView thisTile = (MyTextView)sender;

				Console.WriteLine("\r\r\r this tile is at: \r x={0} y={1}", thisTile.xPos, thisTile.yPos) ;
				Console.WriteLine("\r\r\r empty tile is at: \r x={0} y={1}", emptySpot.X, emptySpot.Y);

				float xDiff = (float)Math.Pow(thisTile.xPos - emptySpot.X, 2);
				float yDiff = (float)Math.Pow(thisTile.yPos - emptySpot.Y, 2);
				float dist = (float)Math.Sqrt(xDiff + yDiff);

				if (dist == 1)
				{
					// tile can move
					// memorize where the tile used to be
					Point currPoint = new Point(thisTile.xPos, thisTile.yPos);

					// now take the tile to the empty
					GridLayout.Spec rowSpec = GridLayout.InvokeSpec(emptySpot.Y);
					GridLayout.Spec colSpec = GridLayout.InvokeSpec(emptySpot.X);

					GridLayout.LayoutParams newLocalParams = new GridLayout.LayoutParams(rowSpec, colSpec);

					thisTile.xPos = emptySpot.X;
					thisTile.yPos = emptySpot.Y;

					newLocalParams.Width = tileWidth - 10;
					newLocalParams.Height = tileWidth - 10;
					newLocalParams.SetMargins(5, 5, 5, 5);


					thisTile.LayoutParameters = newLocalParams;

					emptySpot = currPoint;
				}
			}
		}

		private void RandomizeTiles()
		{
			ArrayList tempCoords = new ArrayList(coordsArray); // 16 elements
			Random myRand = new Random();
			foreach (MyTextView any in tilesArray)	// 15 elements
			{
				int randIndex = myRand.Next(0, tempCoords.Count);
				Point thisRandLoc = (Point)tempCoords[randIndex];

				GridLayout.Spec rowSpec = GridLayout.InvokeSpec(thisRandLoc.Y);
				GridLayout.Spec colSpec = GridLayout.InvokeSpec(thisRandLoc.X);
				GridLayout.LayoutParams randLayoutParam = new GridLayout.LayoutParams(rowSpec, colSpec);


				any.xPos = thisRandLoc.X;
				any.yPos = thisRandLoc.Y;

				randLayoutParam.Width = tileWidth - 10;
				randLayoutParam.Height = tileWidth - 10;
				randLayoutParam.SetMargins(5, 5, 5, 5);


				any.LayoutParameters = randLayoutParam;

				tempCoords.RemoveAt(randIndex);
			}

			Console.WriteLine("\r\r\r\r there are: {0} elements left", tempCoords.Count);
			emptySpot = (Point)tempCoords[0];
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
			RandomizeTiles();
		}
	}

	class MyTextView : TextView
	{
		Activity myContext;

		public MyTextView (Activity context) : base (context)
		{
			myContext = context;
		}

		public int xPos { set; get; }
		public int yPos { set; get; }
	}
}

