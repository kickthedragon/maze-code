using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class MazeGenerator : MonoBehaviour {

	public int width, height;
	public int scale;

	public Material brick;

	public bool is2D;
	public bool isGenerator;
	public bool debugMode;

	public GameObject solvedPieces;

    public GameObject playerPrefab;
	public GameObject playerPrefab2d;
	public GameObject jewelPrefab;
	public GameObject winBrick;


	public GameObject wall2d;
	public GameObject debugQuad;
    public GameObject wall3d;

	private int[,] Maze;
	private Stack<Vector2> _tiletoTry = new Stack<Vector2>();
	private List<Vector2> offsets = new List<Vector2> { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };
	private System.Random rnd = new System.Random();
	private int _width, _height;
	private Vector2 _currentTile;
	public static String MazeString;
    public static string[] MazeStringArray;
    public static string[] SolvedMazeArray;
    public static string SolvedMaze;
	public static bool solvingMaze = false;
	public static bool mazeSolved;
	public static int CorrectSolutionSteps = 0;
    public int StartX {  get { return (int)(width / 2); } }
    public int StartY { get { return (int)(height / 2); } }
	static float bCounter;
	public static float buildProgress { get; private set; }
	static float mCounter;
	public static float mazeStringProgres { get; private set; }
	public static float totalProgress { get { return (mazeStringProgres + buildProgress) / 2; } }

	private GameObject[] blocks;


#if !UNITY_ANDROID
	private int drawRadius = 16;
#endif
#if UNITY_ANDROID
	private int drawRadius = 16;
#endif
	public static event Action OnSolvedMaze;
	static void FireSolvedMaze() {
		if (OnSolvedMaze != null)
			OnSolvedMaze ();
	}
	public static event Action OnGenerated;
	static void FireMazeGenerated(){if (OnGenerated != null)
			OnGenerated ();
		}

	public static event Action<float> OnUpdateGenerateProgress;
	static void FireUpdateProgress(float progress){if (OnUpdateGenerateProgress != null)
		OnUpdateGenerateProgress (progress);
	}

    public Vector2 CurrentTile {
		get { return _currentTile; }
		private set {
			if (value.x < 1 || value.x >= this.width - 1 || value.y < 1 || value.y >= this.height - 1){
				throw new ArgumentException("Width and Height must be greater than 2 to make a maze");
			}
			_currentTile = value;
		}
	}
	private static MazeGenerator instance;
	public static MazeGenerator Instance {
		get {return instance;}
	}
	void Awake()  
	{ 
		if (instance == null) 
			instance = this;
		else
			Destroy (gameObject);
	}

	void OnDestroy()
	{
		if (instance == this)
			instance = null;
	}

    void Start()
    {
		if (Settings.GetResetPrefs() == 1) {
			Settings.ResetPosition();
			Settings.SetResetPrefs(0);
		}
		GenerateMaze();
    }
	void OnEnable()
	{
		PlayerEventManager.OnPlayerToggleDebug += ToggleDebug;
		PlayerEventManager.OnPlayerSpawned += DrawInitialMaze;
		PlayerEventManager.OnPlayerUpdatePosition += DrawMaze;
     //   OnGenerated += startSolveRoutine;
		if(isGenerator)
        	OnGenerated += writeMazeStringToFile;
     //   OnSolvedMaze += writeSolvedMazeStringToFile;
    }

	void OnDisable()
	{
		PlayerEventManager.OnPlayerToggleDebug -= ToggleDebug;
		PlayerEventManager.OnPlayerSpawned -= DrawInitialMaze;
		PlayerEventManager.OnPlayerUpdatePosition -= DrawMaze;
     //   OnGenerated -= startSolveRoutine;
        if(isGenerator)
			OnGenerated -= writeMazeStringToFile;
     //   OnSolvedMaze -= writeSolvedMazeStringToFile;
    }

    

   
    void SetStartingPoint(string[] array)
    {
        var mod = array[StartX]; int index = StartY;
        array[StartX] = mod.Substring(0, index) + 'S' + mod.Substring(index + 1);

    }

    void SetFinishPoint(string[] array)
    {
        var mod = array[0]; int index = 1;
        array[0] = mod.Substring(0, index) + 'F' + mod.Substring(index + 1);
    }

	void GenerateMaze()
	{

		if (isGenerator)
			StartCoroutine (MakeMazeString ());
		else
			ReadMaze ();
		//StartCoroutine(BuildBlock());



		//print (MazeString); 
		
		// added to create String
		// print(MazeStringArray);
		
		
		
		if (debugMode) {
			
			if (solveMaze (StartX, StartY)) {
				foreach (string s in SolvedMazeArray)
					SolvedMaze += s + "\n";
				
				print (SolvedMaze);
				FireSolvedMaze ();
				ShowSolutionPath ();
			} else
				print ("Unsolveable");
			
			print (CorrectSolutionSteps);
			mazeSolved = true;
		}
	}

	void ReadMaze()
	{
		var textFile = Resources.Load("Maze", typeof(TextAsset)) as TextAsset;
		MazeStringArray = textFile.text.Split ('\n');
		StartObjects();
	}

    IEnumerator MakeMazeString() {

		mCounter = 0;
		mazeStringProgres = 0;
        Maze = new int[width, height];
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Maze[x, y] = 1;
            }
        }
        CurrentTile = Vector2.one;
        _tiletoTry.Push(CurrentTile);
        Maze = CreateMaze();  // generate the maze in Maze Array.
       
        for (int i = 0; i <= Maze.GetUpperBound(0); i++) {

            for (int j = 0; j <= Maze.GetUpperBound(1); j++) {
                if (Maze[i, j] == 1) {
                    MazeString = MazeString + "X";  
                }
                else if (Maze[i, j] == 0) {
                    MazeString = MazeString + " "; // added to create String
                }
				mCounter++;
				mazeStringProgres = (mCounter / (Maze.GetUpperBound(0) * Maze.GetUpperBound(1)));
				FireUpdateProgress(mazeStringProgres);
            }
			mCounter++;
            MazeString = MazeString + "\n";  // added to create String
			mazeStringProgres = (mCounter / (Maze.GetUpperBound(0) * Maze.GetUpperBound(1)));
			FireUpdateProgress(mazeStringProgres);
			yield return null;
        }

        MazeStringArray = MazeString.Split("\n"[0]);
        SetStartingPoint(MazeStringArray);
        SetFinishPoint(MazeStringArray);
        MazeString = string.Empty;
		SolvedMazeArray = MazeStringArray;
		for (int i =0; i < MazeStringArray.Length; i++)
        {
            MazeString += MazeStringArray[i] + "\n";
        }

		bCounter = 0;
		buildProgress = 0;

		StartObjects ();

		//yield return StartCoroutine ("BuildAllBlocks");
       
	}

	void ToggleDebug()
	{
		if (!mazeSolved) {
			
			if (solveMaze (StartX, StartY)) {
				foreach (string s in SolvedMazeArray)
					SolvedMaze += s + "\n";
				
				print (SolvedMaze);
				FireSolvedMaze ();
				ShowSolutionPath ();
			} else
				print ("Unsolveable");
			
			print (CorrectSolutionSteps);
			mazeSolved = true;
			solToggle = true;
		}
		else
		{
			ToggleSolutionPath();
		}
	}

    void startSolveRoutine()
    {
        solvingMaze = true;
        StartCoroutine(solveMazeRoutine());
    }

	public IEnumerator solveMazeRoutine()
	{

        int r = StartX;
        int c = StartY;
           
		do 
		{
          

            // write solving method iteration instead of recursion method here


            yield return new WaitForEndOfFrame();
		} while(solvingMaze && !mazeSolved);

        foreach (string s in SolvedMazeArray)
        	SolvedMaze += s + "\n";

        mazeSolved = true;
        FireSolvedMaze();

        //print(SolvedMaze);
        //ShowSolutionPath();
    }

    public static bool solveMaze(int r, int c)
    {
       
        if (r < 0 || c < 0 || r >= SolvedMazeArray.Length || c >= SolvedMazeArray[0].Length)
            return false;
        if (SolvedMazeArray [r] [c] == 'F') {
			CorrectSolutionSteps++;
			return true;
		}
       
        if (SolvedMazeArray[r][c] != ' ' && SolvedMazeArray[r][c] != 'S')
            return false;
    
        SolvedMazeArray[r] = SolvedMazeArray[r].Substring(0, c) + 'A' + SolvedMazeArray[r].Substring(c + 1);

        if (solveMaze(r - 1, c))
        {
            SolvedMazeArray[r] = SolvedMazeArray[r].Substring(0, c) + '#' + SolvedMazeArray[r].Substring(c + 1);
			CorrectSolutionSteps++;
            return true;
        }
        if (solveMaze(r + 1, c))
        {
            SolvedMazeArray[r] = SolvedMazeArray[r].Substring(0, c) + '#' + SolvedMazeArray[r].Substring(c + 1);
			CorrectSolutionSteps++;
            return true;
        }
        if (solveMaze(r, c - 1))
        {
            SolvedMazeArray[r] = SolvedMazeArray[r].Substring(0, c) + '#' + SolvedMazeArray[r].Substring(c + 1);
			CorrectSolutionSteps++;
            return true;
        }
        if (solveMaze(r, c + 1))
        {
            SolvedMazeArray[r] = SolvedMazeArray[r].Substring(0, c) + '#' + SolvedMazeArray[r].Substring(c + 1);
			CorrectSolutionSteps++;
            return true;
        }

        return false;
    }

	void StartObjects()
	{
		if (MazeStringArray [((width - 1) / 2)].Substring (((width - 1) / 2), 1) == "S") 
		{
			GameObject go = GameObject.Instantiate(playerPrefab);
			
			if(GameManager.isRetrying || Settings.GetPlayerPosition () == string.Empty)
				go.transform.position = new Vector3(((width - 1) / 2)* scale, .5f + (scale / 2), ((width - 1) / 2) * scale);
			else
				go.transform.position = Settings.ParseVector3String(Settings.GetPlayerPosition());
			
			go.transform.localScale = new Vector3(scale * .5f, scale * .5f, scale * .5f);
		}
		if(MazeStringArray[0].Substring(1, 1) == "F")
		{
			GameObject go = GameObject.Instantiate(winBrick);
			go.transform.position = new Vector3(1 * scale, .5f + (scale / 2), 0 * scale);
			go.transform.localScale = new Vector3(scale, scale, scale);
			go.transform.parent = transform;
		}
	
	}

	void DrawInitialMaze(float x, float z)
	{
		int xCord = convertPositionToCoordinate (x);
		int yCord = convertPositionToCoordinate (z);

		//Debug.Log (xCord +","+ yCord);

		int drawXMin = xCord - drawRadius;
		int drawYMin = yCord - drawRadius;

		blocks = new GameObject[drawRadius * drawRadius*drawRadius];
		for(int b = 0; b < blocks.Length; b++)
		{
			if(is2D)
				blocks[b] = GameObject.Instantiate(wall2d);
			else
				blocks[b] = GameObject.Instantiate(wall3d);
		}

		int cnt = 0;
		for (int i = drawYMin; i < ((yCord+drawRadius) > width-1 ? (width-1):(yCord+drawRadius)); i++) 
		{
			for (int j = drawXMin; j < ((xCord+drawRadius) > width-1 ? (width-1):(xCord+drawRadius)); j++)
			{
				string st = MazeStringArray[i];
				if(st.Substring(j, 1) == "X")
				{

					//blocks[cnt] = GameObject.Instantiate(wall3d);
				                
					blocks[cnt].transform.position = new Vector3(j * scale, .5f+(scale/2), i *scale);
					blocks[cnt].transform.localScale = new Vector3(scale, scale, scale);
					blocks[cnt].name = "["+i+","+j+"]";
					blocks[cnt].transform.parent = transform;
					cnt++;
				}
			}

		}



		FireMazeGenerated ();

	}

	void DrawMaze(float x, float z)
	{
		int xCord = convertPositionToCoordinate (x);
		int yCord = convertPositionToCoordinate (z);

//		for (int cnt = 0; cnt < transform.childCount; cnt++) {
//			DestroyImmediate(transform.GetChild(cnt).gameObject);
//		}

		int cntr = 0;
		int drawXMin = xCord - drawRadius;
		int drawYMin = yCord - drawRadius;
		
		for (int i = drawYMin; i < ((yCord+drawRadius) > width-1 ? (width-1):(yCord+drawRadius)); i++) 
		{
			for (int j = drawXMin; j < ((xCord+drawRadius) > width-1 ? (width-1):(xCord+drawRadius)); j++)
			{
				string st = MazeStringArray[i];
				string goName = "["+i+","+j+"]";

				if(st.Substring(j, 1) == "X")
				{
					//GameObject ptype = GameObject.Instantiate(wall3d);				               
					blocks[cntr].transform.position = new Vector3(j * scale, .5f+(scale/2), i *scale);
					blocks[cntr].transform.localScale = new Vector3(scale, scale, scale);
					blocks[cntr].name = goName;
					blocks[cntr].transform.parent = transform;
					cntr++;
				}


			}
//			/cntr++;
		}
	}

	int convertPositionToCoordinate(float pos)
	{
		return Mathf.FloorToInt(pos / scale);
	}




    IEnumerator BuildAllBlocks()
    {

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                string st = MazeStringArray[i];
                if(st.Substring(j, 1) == "X")
                {
					GameObject ptype = GameObject.Instantiate(wall3d);					                       
					ptype.transform.position = new Vector3(j * scale, .5f+(scale/2), i *scale);
					ptype.transform.localScale = new Vector3(scale, scale, scale);
					ptype.name = "["+i+","+j+"]";
                    ptype.transform.parent = transform;
                }
                if(st.Substring(j, 1) == "S")
                {
					GameObject go = GameObject.Instantiate(playerPrefab);

					if(GameManager.isRetrying || Settings.GetPlayerPosition () == string.Empty)
	                    go.transform.position = new Vector3(j * scale, .5f + (scale / 2), i * scale);
					else
						go.transform.position = Settings.ParseVector3String(Settings.GetPlayerPosition());

					go.transform.localScale = new Vector3(scale * .5f, scale * .5f, scale * .5f);
                }

				if(st.Substring(j, 1) == "F")
				{
					GameObject go = GameObject.Instantiate(winBrick);
					go.transform.position = new Vector3(j * scale, .5f + (scale / 2), i * scale);
					go.transform.localScale = new Vector3(scale, scale, scale);
					go.transform.parent = transform;
				}
				bCounter ++;
				buildProgress = (bCounter/ (width*height));
				FireUpdateProgress(totalProgress);
				//Debug.Log(buildProgress);

            }
			bCounter++;
			buildProgress = (bCounter/ (width*height));
			FireUpdateProgress(totalProgress);
			//Debug.Log(buildProgress);
			yield return null;
        }

		FireMazeGenerated ();

		//StartCoroutine (CombineMehes ());
    }

    static void writeSolvedMazeStringToFile()
    {
        string destination = string.Empty;
#if UNITY_STANDALONE_WIN
        destination = "C:/SolvedMaze.txt";
#endif
#if UNITY_STANDALONE_OSX
        destination = "~/SolvedMaze.txt";
#endif

        System.IO.File.WriteAllLines(destination, SolvedMazeArray);
    }

    static void writeMazeStringToFile()
    {

        string destination = string.Empty;
#if UNITY_STANDALONE_WIN
        destination = "C:/Maze.txt";
#endif
#if UNITY_STANDALONE_OSX || UNITY_EDITOR
        destination = Application.persistentDataPath+"/Maze.txt";
#endif

        System.IO.File.WriteAllLines(destination, MazeStringArray);
    }


    IEnumerator CombineMehes()
	{
		
		// MAX MESHES I CAN COMBINE IS 2592 at a time
		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter> ();

		 
		int k = 0;
		while (k < meshFilters.Length) {

			CombineInstance[] combine;

			if((k+2592) > meshFilters.Length)
			{
				combine = new CombineInstance[(meshFilters.Length-k)];
			}
			else
				combine = new CombineInstance[2592];
			int j = 0;

			while (j < combine.Length) {
				combine [j].mesh = meshFilters [k + j].sharedMesh;
				combine [j].transform = meshFilters [j + k].transform.localToWorldMatrix;
				meshFilters [j].gameObject.SetActive (false);
				j++;
				yield return null;
			}

			transform.GetComponent<MeshFilter> ().mesh = new Mesh ();
			transform.GetComponent<MeshFilter> ().mesh.CombineMeshes (combine);
			transform.GetComponent<MeshRenderer> ().material = brick;
			transform.gameObject.SetActive (true);

			if((k+2592) > meshFilters.Length)
			{
				k+=(meshFilters.Length-k);
			}
			else
				k += 2592;

			yield return null;
		}
	
		for (int l = 0; l < transform.childCount; l++)
		{
			//Destroy(transform.GetChild(l).gameObject);
		}


	}

    void ShowSolutionPath()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                string st = SolvedMazeArray[i];
              
                if (st.Substring(j, 1) == "#")
                {
                    GameObject go = GameObject.Instantiate(debugQuad);
                    go.GetComponent<Renderer>().material.color = Color.red;
                    go.transform.position = new Vector3(j * scale, .5f + (scale / 2), i * scale);
					go.transform.parent = solvedPieces.transform;
                }

                if (st.Substring(j, 1) == "A")
                {
                    GameObject go = GameObject.Instantiate(debugQuad);
                    go.GetComponent<Renderer>().material.color = Color.blue;
                    go.transform.position = new Vector3(j * scale, .5f + (scale / 2), i * scale);
					go.transform.parent = solvedPieces.transform;
                }


            }
        }
    }

	bool solToggle;
	public void ToggleSolutionPath()
	{
		solToggle = !solToggle;
		solvedPieces.SetActive (solToggle);
	}

	// =======================================
	public int[,] CreateMaze() {
		
		//local variable to store neighbors to the current square as we work our way through the maze
		List<Vector2> neighbors;
		//as long as there are still tiles to try
		while (_tiletoTry.Count > 0)
		{
			//excavate the square we are on
			Maze[(int)CurrentTile.x, (int)CurrentTile.y] = 0;
			//get all valid neighbors for the new tile
			neighbors = GetValidNeighbors(CurrentTile);
			//if there are any interesting looking neighbors
			if (neighbors.Count > 0)
			{
				//remember this tile, by putting it on the stack
				_tiletoTry.Push(CurrentTile);
				//move on to a random of the neighboring tiles
				CurrentTile = neighbors[rnd.Next(neighbors.Count)];
			}
			else
			{
				//if there were no neighbors to try, we are at a dead-end toss this tile out
				//(thereby returning to a previous tile in the list to check).
				CurrentTile = _tiletoTry.Pop();
			}
		}
		//print("Maze Generated ...");
		return Maze;
	}
	
	// ================================================
	// Get all the prospective neighboring tiles "centerTile" The tile to test
	// All and any valid neighbors</returns>
	private List<Vector2> GetValidNeighbors(Vector2 centerTile) {
		List<Vector2> validNeighbors = new List<Vector2>();
		//Check all four directions around the tile
		foreach (var offset in offsets) {
			//find the neighbor's position
			Vector2 toCheck = new Vector2(centerTile.x + offset.x, centerTile.y + offset.y);
			//make sure the tile is not on both an even X-axis and an even Y-axis
			//to ensure we can get walls around all tunnels
			if (toCheck.x % 2 == 1 || toCheck.y % 2 == 1) {
				
				//if the potential neighbor is unexcavated (==1)
				//and still has three walls intact (new territory)
				if (Maze[(int)toCheck.x, (int)toCheck.y]  == 1 && HasThreeWallsIntact(toCheck)) {
					
					//add the neighbor
					validNeighbors.Add(toCheck);
				}
			}
		}
		return validNeighbors;
	}
	// ================================================
	// Counts the number of intact walls around a tile
	//"Vector2ToCheck">The coordinates of the tile to check
	//Whether there are three intact walls (the tile has not been dug into earlier.
	private bool HasThreeWallsIntact(Vector2 Vector2ToCheck) {
		
		int intactWallCounter = 0;
		//Check all four directions around the tile
		foreach (var offset in offsets) {
			
			//find the neighbor's position
			Vector2 neighborToCheck = new Vector2(Vector2ToCheck.x + offset.x, Vector2ToCheck.y + offset.y);
			//make sure it is inside the maze, and it hasn't been dug out yet
			if (IsInside(neighborToCheck) && Maze[(int)neighborToCheck.x, (int)neighborToCheck.y] == 1) {
				intactWallCounter++;
			}
		}
		//tell whether three walls are intact
		return intactWallCounter == 3;
	}
	
	// ================================================
	private bool IsInside(Vector2 p) {
		//return p.x >= 0  p.y >= 0  p.x < width  p.y < height;
		return p.x >= 0 && p.y >= 0 && p.x < width && p.y < height;
	}
}