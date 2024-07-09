using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections;

namespace DataProvider
{
	public class Provider
	{
		private XDocument players;
		private XDocument games;
		private string playersFilename;
		private string gamesFilename;
		private string dataFilename = "data.xml";

		public Provider(string playersFilename, string gamesFilename)
		{
			this.playersFilename = playersFilename;
			this.gamesFilename = gamesFilename;
			if (File.Exists(playersFilename))
			{
				players = XDocument.Load(playersFilename);
			}
			else
			{
				players = new XDocument(
					new XElement("players",
						new XAttribute("maxId", "0"),
						new XElement("player",
							new XAttribute("id", "0"),
							new XAttribute("name", ""),
							new XAttribute("password", ""))
						)
					);
				File.Create(playersFilename);
				players.Save(playersFilename);
			}

			if (File.Exists(gamesFilename))
			{
				games = XDocument.Load(gamesFilename);
			}
			else
			{
				File.Create(gamesFilename);
				games = new XDocument(new XElement("games",
							new XAttribute("maxId", "0")));
				games.Save(gamesFilename);
			}
		}

		public bool Register(string name, string password)
		{
			if (LogIn(name, password).Id == 0)
			{
				int maxId = Convert.ToInt16(players.Root.Attribute("maxId").Value) + 1;
				XElement player = new XElement("player",
										new XAttribute("id", maxId),
										new XAttribute("name", name),
										new XAttribute("password", password));
				players.Root.Add(player);
				players.Root.Attribute("maxId").Value = Convert.ToString(maxId);
				players.Save(playersFilename);
				return true;
			}
			else
			{
				return false;
			}
		}

		public Player LogIn(string name, string password)
		{
			foreach (XElement player in players.Root.Elements())
			{
				if (player.Attribute("name").Value == name && player.Attribute("password").Value == password)
					return new Player(Convert.ToInt16(player.Attribute("id").Value), player.Attribute("name").Value, player.Attribute("password").Value);
			}
			return new Player();
		}

		public void SaveGameResult(int playerId, int gameId)
		{
			FileInfo file = new FileInfo(dataFilename);
			if (file.Exists)
			{
				XDocument data = XDocument.Load(dataFilename);
				int maxId = Convert.ToInt16(games.Root.Attribute("maxId").Value) + 1;
				XElement game = new XElement("game");
				if (gameId == 0)
				{
					game = new XElement("game",
						new XAttribute("id", maxId),
						new XAttribute("playerId", playerId),
						new XAttribute("points", data.Root.Attribute("points").Value));
				}
				else
				{
					foreach (XElement el in games.Root.Elements())
					{
						if (el.Attribute("id").Value == Convert.ToString(gameId))
						{
							el.Remove();
							break;
						}
					}
					game = new XElement("game",
						new XAttribute("id", gameId),
						new XAttribute("playerId", playerId),
						new XAttribute("points", data.Root.Attribute("points").Value));
				}
				foreach (XElement el in data.Root.Elements())
				{
					game.Add(el);
				}
				games.Root.Add(game);
				games.Root.Attribute("maxId").Value = Convert.ToString(maxId);
				games.Save(gamesFilename);
			}
			else
				throw new FileNotFoundException(dataFilename);
			return;
		}

		public List<string> GetRecords()
		{
			IEnumerable<List<string>> records = games.Root.Elements().Select(game => new List<string> { game.Attribute("playerId").Value, game.Attribute("points").Value }).OrderByDescending(record => Convert.ToInt16(record.Last()));
			List<string> result = new List<string>();
			foreach (List<string> record in records)
			{
				foreach (XElement player in players.Root.Elements())
				{
					if (player.Attribute("id").Value == record.First())
						result.Add(player.Attribute("name").Value + ": " + record.Last());
				}
			}
			return result;
		}
	}
}
