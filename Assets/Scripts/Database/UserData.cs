using System;
using ProjectW.DB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData
{
    public DtoAccount dtoAccount;
    public DtoCharacter dtoCharacter;
    public DtoStage dtoStage;
    public DtoInventory dtoInventory;

    public UserData(string nickname)
    {
        dtoAccount = new DtoAccount(nickname);
        dtoCharacter = new DtoCharacter();
        dtoStage = new DtoStage();
        dtoInventory = new DtoInventory();
    }
}
