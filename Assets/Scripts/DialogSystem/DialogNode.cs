using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogNode
{
    Result Evaluate(PlayerData playerData, CodingMamaCharacter characterData)
    {
        return new Result();
    }

    public void Restart()
    {
        // todo: restart variables to initial state
    }
}

[Serializable]
public class Result
{
    public bool isDone = true;
    [FormerlySerializedAs("isANonResponse")] [FormerlySerializedAs("isANonRespones")] public bool isAnActiveResponse = false;
}

public class ResultFactory
{
    public static Result CreateEndingResult()
    {
        Result result = new Result();
        result.isAnActiveResponse = true;
        result.isDone = true;
        return result;
    }
    
    public static Result CreateStartingResult()
    {
        Result result = new Result();
        result.isAnActiveResponse = true;
        result.isDone = false;
        return result;
    }

    public static Result CreateNonReponseResult()
    {
        Result result = new Result();
        result.isAnActiveResponse = false;
        result.isDone = true;
        return result;
    }

    public static Result CreateChattingResult()
    {
        return CreateStartingResult();
    }
}