using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IDiscussion
{
    void sayTruth();
    void addPlayerStatement();
    void ask();
    void askPlayer(PlayerBase player);
    void askRandomPlayer();

}
