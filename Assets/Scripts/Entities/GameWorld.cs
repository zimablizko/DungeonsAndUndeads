using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld
{
    private List<Room> roomList;
    private Room currentRoom;

    public Room CurrentRoom
    {
        get => currentRoom;
        set => currentRoom = value;
    }

    public GameWorld()
    {
        this.roomList = new List<Room>();
        this.currentRoom = null;
        GenerateGameWorld();
    }

    public void AddRoom(Room room)
    {
        roomList.Add(room);
    }

    public Room GetNextRoom()
    {
        return roomList[roomList.IndexOf(currentRoom) + 1];
    }    
    
    public Room GetFirstRoom()
    {
        return roomList[0];
    }
    
    public void SetNextRoom()
    {
        currentRoom = roomList[roomList.IndexOf(currentRoom) + 1];
    }

    private void GenerateGameWorld()
    {
        // AddRoom(new Room("room_10"));
        AddRoom(new Room("room_0"));
        AddRoom(new Room(new [] {"room_1_1", "room_1_2"}));
        AddRoom(new Room(new [] {"room_2_1", "room_2_2"}));
        AddRoom(new Room("room_treasure"));
        AddRoom(new Room(new [] {"room_4_1","room_4_2","room_4_3"}));
        AddRoom(new Room(new [] {"room_5_1","room_5_2"}));
        AddRoom(new Room("room_treasure"));
        AddRoom(new Room(new [] {"room_7_1","room_7_2"}));
        AddRoom(new Room(new [] {"room_8_1","room_8_2"}));
        AddRoom(new Room("room_restoration"));
        AddRoom(new Room("room_10"));
        currentRoom = roomList[0];
    }
}
