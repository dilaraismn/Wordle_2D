using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
   [SerializeField] private GameObject notValidUI, newWordButton;
   private static readonly KeyCode[] SUPPORTED_KEYS = new KeyCode[] 
   {
      KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F,
      KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L,
      KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R,
      KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X,
      KeyCode.Y, KeyCode.Z,
   };
   
   
   public string[] solutions;
   public string[] validWords;
   private Row[] rows;
   private int rowIndex, columnIndex;
   private string word;
   
   public Tile.State emptyState, occupiedState, correctState, wrongSpotState, incorrectState;

   private void Awake()
   {
      rows = GetComponentsInChildren<Row>();
   }

   private void Start()
   {
      if (UIManager.isTurkish)
      {
         LoadData("turkish_words", "allTurkishWords");
      }
      else
      {
         LoadData("official_wordle_common", "official_wordle_all");
      }
      NewGame();
   }

   private void Update()
   {
      Row currentRow = rows[rowIndex];
      
      if (Input.GetKeyDown(KeyCode.Backspace))
      {
         columnIndex = Mathf.Max(columnIndex - 1, 0);
         currentRow.tiles[columnIndex].SetLetter('\0');
         currentRow.tiles[columnIndex].SetState(emptyState);
         notValidUI.SetActive(false);
      }
      else if (columnIndex >= currentRow. tiles.Length)
      {
         if (Input.GetKeyDown(KeyCode.Return))
         {
            SubmitRow(currentRow);  
         }
      }
      else
      {
         for (int i = 0; i < SUPPORTED_KEYS.Length; i++)
         {
            if (Input.GetKeyDown(SUPPORTED_KEYS[i]))
            {
               currentRow.tiles[columnIndex].SetLetter((char) SUPPORTED_KEYS[i]);
               currentRow.tiles[columnIndex].SetState(occupiedState);
               columnIndex++;
               break;
            }
         }
      }
   }

   private void SetRandomWord()
   {
      word = solutions[Random.Range(0, solutions.Length)];
      word = word.ToLower().Trim(); //make thme lower case and trim any blanks
      //word = "ugurc";
      print(word);
   }

   private void LoadData(string solutionFile, string allWordsFile)
   {
      
      TextAsset textFile = Resources.Load(solutionFile) as TextAsset;
      solutions = textFile.text.Split('\n');

      textFile = Resources.Load(allWordsFile) as TextAsset;
      validWords = textFile.text.Split('\n');
   }

   private void SubmitRow(Row row)
   {
      if (!IsValidWord(row.word))
      {
         notValidUI.SetActive(true);
         return;
      }
      string remaining = word;
      for (int i = 0; i < row.tiles.Length; i++)
      {
         Tile tile = row.tiles[i];

         if (tile.letter == word[i])
         {
            tile.SetState(correctState);
            remaining = remaining.Remove(i, 1);
            remaining = remaining.Insert(i, " ");
         }
         else if (!word.Contains(tile.letter.ToString()))
         {
            tile.SetState(incorrectState);
         }
      }

      for (int i = 0; i < row.tiles.Length; i++)
      {
         Tile tile = row.tiles[i];

         if (tile.state != correctState && tile.state != incorrectState)
         {
            if (remaining.Contains(tile.letter.ToString())) //check if there is a second instance of that letter
            {
               tile.SetState(wrongSpotState);
               //check if there is a thitd instance
               int index = remaining.IndexOf(tile.letter);
               remaining = remaining.Remove(index, 1);
               remaining = remaining.Insert(i, " ");
            }
            else
            {
               tile.SetState(incorrectState);
            }
         }
      }

      if (IsWin(row))
      {
         enabled = false;
      }
      
      rowIndex++;
      columnIndex = 0;

      if (rowIndex >= rows.Length)
      {
         enabled = false;
      }
   }

   private bool IsValidWord(string word)
   {
      // int trueCount = 0;
      //
      // foreach (var haydar in validWords)
      // {
      //    for (int i = 0; i < 5; i++)
      //    {
      //       if (haydar[i] == word[i])
      //       {
      //          trueCount++;
      //       }
      //    }
      //
      //    if (trueCount == 5)
      //    {
      //       return true;
      //    }
      // }
      //
      // return false;
      
      
      // if (validWords.Contains(word))
      // {
      //    Debug.Log("haydar1");
      //    return true;
      // }
      // else
      // {
      //    Debug.Log("haydar2");
      //    return false;
      // }
      
      for (int i = 0; i < validWords.Length; i++)
      {
         if (validWords[i].StartsWith(word))
         {
            return true;
         }
      }
      
      return false;
   }

   private bool IsWin(Row row)
   {
      for (int i = 0; i < row.tiles.Length; i++)
      {
         if (row.tiles[i].state != correctState)
         {
            return false;
         }
      }
      return true;
   }

   void ClearBoard()
   {
      for (int row = 0; row < rows.Length; row++)
      {
         for (int column = 0; column < rows[row].tiles.Length; column++)
         {
            rows[row].tiles[column].SetLetter('\0');
            rows[row].tiles[column].SetState(emptyState);
         }
      }

      rowIndex = 0;
      columnIndex = 0;
   }
   
   private void OnEnable()
   {
      newWordButton.SetActive(false);
   }

   private void OnDisable()
   {
      newWordButton.SetActive(true);
   }
   
   public void NewGame()
   {
      ClearBoard();
      SetRandomWord();
      enabled = true;
   }
}
