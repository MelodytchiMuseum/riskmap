﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;

namespace SUOnlineRisk
{
    [Serializable]
    public class Map
    {
        // Variables.
        protected List<Continent> continents;
        protected List<Territory> territories;
        protected List<ReinforcementCard> cards;
        protected string fileName;
        protected Bitmap bitmap;

        // Constructor.
        public Map(string file)
        {
            this.fileName = file;
            this.continents = new List<Continent>();
            this.territories = new List<Territory>();
            this.cards = new List<ReinforcementCard>();
            /*try
            {
                bitmap = new Bitmap(this.fileName);
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.Error.WriteLine("Map: File " + this.fileName + " does not exists.");
            }*/
        }

        public Map(Bitmap bitmap)
        {
            this.fileName = "Not given";
            this.continents = new List<Continent>();
            this.territories = new List<Territory>();
            this.bitmap = (Bitmap) bitmap.Clone();
        }

        //add a continent
        public void addContinent(Continent c)
        {
            continents.Add(c);
        }

        //add a territory
        public bool addTerritory(Territory t, Continent c)
        {
            if(continents.Exists(x => x.getName()==c.getName()))
            {
                Continent continent = continents.Find(x=>x.getName()==c.getName());
                continent.addTerritory(t);
                territories.Add(t);
                return true;
            }
            else
            {
                return false;
            }
        }

        // Adding a card.
        public void addCard(ReinforcementCard temp) {
            cards.Add(temp);
        }

        /*
         * find a territory by its name.
         * return null if there is no such territory.
         */
        public Territory getTerritory(string name)
        {
            if(territories.Exists(x=> x.getName()==name))
            {
                return territories.Find(x => x.getName() == name);
            }
            else
            {
                return null;
            }
        }

        // Returning a specific continent.
        public Continent getOneContinent(int i) { return continents.ElementAt(i); }

        // Returning a specific territory.
        public Territory getOneTerritory(int i) { return territories.ElementAt(i); }

        // Returning all continents.
        public List<Continent> getAllContinents() { return continents; }

        // Returning all territories.
        public List<Territory> getAllTerritories() { return territories; }

        // Retrive the bitmap
        public Bitmap getBitmap() { return bitmap; }

        // Retrieving list of reinforcement cards.
        public List<ReinforcementCard> getAllCards(int i) { return cards; }

        // Retrieving a reinforcement card.
        public ReinforcementCard getCard(int i) { return cards.ElementAt(i); }

        // Saving the map.
        public void saveMap(string path)
        {
            Stream stream = null;
            try
            {
                stream = File.Create(path);
            }
            catch(System.IO.DirectoryNotFoundException)
            {
                Console.Error.WriteLine("Path, " + path + ", does not exist.");
            }
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(stream, this); 
            }
            catch(System.Runtime.Serialization.SerializationException)
            {
                Console.Error.WriteLine("Serialization failed. Could not save the map");
            }
            finally
            {
                stream.Close();
            }
        }

        // Loading the necessary files and deciphering the mapcode.
        public static Map loadMap(string path)
        {
            Stream stream = null;
            try
            {
                stream = File.OpenRead(path);
            }
            catch(Exception ex)
            {
                if(ex is System.IO.DirectoryNotFoundException || ex is System.IO.FileNotFoundException)
                {
                    Console.Error.WriteLine("Path, " + path + ", does not exist.");
                }
                throw;
            }
            BinaryFormatter formatter = new BinaryFormatter();
            Map map = null;
            try
            {
                map = (Map) formatter.Deserialize(stream); 
            }
            catch(System.Runtime.Serialization.SerializationException)
            {
                //Console.Error.WriteLine("Serialization failed. Could not save the map");
                throw;         
            }
            finally
            {
                stream.Close();
            }
            return map;
        }
    }
}