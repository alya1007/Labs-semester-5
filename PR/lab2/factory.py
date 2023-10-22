from player import Player
from typing import List
import xml.etree.ElementTree as ET
import player_pb2 as PlayersList


class PlayerFactory:
    def to_json(self, players: List[Player]):
        return list(
            map(
                lambda player: {
                    "nickname": player.nickname,
                    "email": player.email,
                    "date_of_birth": player.date_of_birth.strftime("%Y-%m-%d"),
                    "xp": player.xp,
                    "class": player.cls,
                },
                players,
            )
        )

    def from_json(self, list_of_dict):
        players = []
        for player_dict in list_of_dict:
            player = Player(
                player_dict["nickname"],
                player_dict["email"],
                player_dict["date_of_birth"],
                player_dict["xp"],
                player_dict["class"],
            )
            players.append(player)
        return players

    def from_xml(self, xml_string: str):
        xml = ET.ElementTree(ET.fromstring(xml_string))
        players_nodes = xml.getroot().iter("player")
        arr = []
        for player_node in players_nodes:
            nickname = player_node.find("nickname").text
            email = player_node.find("email").text
            date_of_birth = player_node.find("date_of_birth").text
            xp = int(player_node.find("xp").text)
            cls = player_node.find("class").text
            arr.append(
                Player(
                    nickname=nickname,
                    email=email,
                    date_of_birth=date_of_birth,
                    xp=xp,
                    cls=cls,
                )
            )

        return arr

    def to_xml(self, list_of_players):
        root = ET.Element("data")  # Create the root element once
        for player in list_of_players:
            player_root = ET.SubElement(root, "player")
            ET.SubElement(player_root, "nickname").text = player.nickname
            ET.SubElement(player_root, "email").text = player.email
            ET.SubElement(
                player_root, "date_of_birth"
            ).text = player.date_of_birth.strftime("%Y-%m-%d")
            ET.SubElement(player_root, "xp").text = str(player.xp)
            ET.SubElement(player_root, "class").text = player.cls

        # Convert the root element to a string
        xml_string = ET.tostring(root, encoding="unicode")
        return xml_string

    def from_protobuf(self, binary):
        player_list = PlayersList.PlayersList()
        player_list.ParseFromString(binary)
        players_obj_list = []
        for player in player_list.player:
            players_obj_list.append(
                Player(
                    player.nickname,
                    player.email,
                    player.date_of_birth,
                    player.xp,
                    PlayersList.Class.Name(player.cls),
                )
            )

        return players_obj_list

    def to_protobuf(self, list_of_players):
        players_list = PlayersList.PlayersList()
        for player in list_of_players:
            players_list.player.add(
                nickname=player.nickname,
                email=player.email,
                date_of_birth=player.date_of_birth.strftime("%Y-%m-%d"),
                xp=player.xp,
                cls=PlayersList.Class.Value(player.cls),
            )

        return players_list.SerializeToString()
