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
        AddRoom(new Room("room_0"));
        AddRoom(new Room(new [] {"room_1_1", "room_1_2"}));
        AddRoom(new Room(new [] {"room_2_1"}));
        AddRoom(new Room("room_treasure"));
        AddRoom(new Room("room_0"));
        currentRoom = roomList[0];
    }
}
