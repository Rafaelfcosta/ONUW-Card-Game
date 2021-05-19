using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IDiscussion
{
    void say();
    void sayTruth();
    void bluff();
    void addPlayerStatement(string text);
    void ask();
    void askPlayer(PlayerBase player);
    void askRandomPlayer();
}
