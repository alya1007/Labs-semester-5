from player import Player
import xml.etree.ElementTree as ET


class PlayerFactory:
    def to_json(self, players):
        # convert list of Player objects to list of dictionaries

        for player in players:
            player_list = []
        for player in players:
            player_dict = vars(player)
            player_dict["date_of_birth"] = player_dict["date_of_birth"].strftime("%Y-%m-%d")
            player_list.append(player_dict)
        return player_list

    def from_json(self, list_of_dict):
        '''
            This function should transform a list of dictionaries into a list with Player objects.
        '''
        players = []
        for player_dict in list_of_dict:
            player = Player(
                player_dict["nickname"],
                player_dict["email"],
                player_dict["date_of_birth"],
                player_dict["xp"],
                player_dict["class"]
            )
            players.append(player)
        return players

    def from_xml(self, xml_string):
        '''
            This function should transform a XML string into a list with Player objects.
        '''
        players = []
        root = ET.fromstring(xml_string)
        for player_elem in root.findall("player"):
            nickname = player_elem.find("nickname").text
            email = player_elem.find("email").text
            date_of_birth = player_elem.find("date_of_birth").text
            xp = int(player_elem.find("xp").text)
            cls = player_elem.find("class").text
            player = Player(nickname, email, date_of_birth, xp, cls)
            players.append(player)
        return players

    def to_xml(self, list_of_players):
        '''
            This function should transform a list with Player objects into a XML string.
        '''
        root = ET.Element("data") # create root element
        for player in list_of_players:
            player_root = ET.SubElement(root, "player")
            ET.SubElement(player_root, "nickname").text = player.nickname
            ET.SubElement(player_root, "email").text = player.email
            ET.SubElement(player_root, "date_of_birth").text = player.date_of_birth.strftime("%Y-%m-%d")
            ET.SubElement(player_root, "xp").text = str(player.xp)
            ET.SubElement(player_root, "class").text = player.cls
        xml_string = ET.tostring(root, encoding="unicode")
        return xml_string

    def from_protobuf(self, binary):
        '''
            This function should transform a binary protobuf string into a list with Player objects.
        '''
        pass

    def to_protobuf(self, list_of_players):
        '''
            This function should transform a list with Player objects intoa binary protobuf string.
        '''
        pass

