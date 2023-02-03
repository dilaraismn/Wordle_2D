using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
   private static readonly KeyCode[] SUPPORTED_KEYS = new KeyCode[] 
   {
      KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F,
      KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L,
      KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R,
      KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X,
      KeyCode.Y, KeyCode.Z,
   };

   private string[] solutions;
   private string[] validWord;
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
      LoadData();
      SetRandomWord();
   }

   
   private void Update()
   {
      Row currentRow = rows[rowIndex];
      
      if (Input.GetKeyDown(KeyCode.Backspace))
      {
         columnIndex = Mathf.Max(columnIndex - 1, 0);
         currentRow.tiles[columnIndex].SetLetter('\0');
         currentRow.tiles[columnIndex].SetState(emptyState);
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
      print(word);
   }
   
   private void LoadData()
   {
      TextAsset TextFile = Resources.Load("official_wordle_all") as TextAsset;
      validWord = TextFile.text.Split('\n');
      
      TextFile = Resources.Load("official_wordle_common") as TextAsset;
      solutions = TextFile.text.Split('\n');
   }
   
   private void SubmitRow(Row row)
   {
      for (int i = 0; i < row.tiles.Length; i++)
      {
         Tile tile = row.tiles[i];
         if (tile.letter == word[i])
         {
            tile.SetState(correctState);
         }
         else if (word.Contains(tile.letter.ToString()))
         {
            tile.SetState(wrongSpotState);
         }
         else
         {
            tile.SetState(incorrectState);
         }
      }

      rowIndex++;
      columnIndex = 0;

      if (rowIndex >= rows.Length)
      {
         //Fail
         enabled = false;
      }
   }
}
