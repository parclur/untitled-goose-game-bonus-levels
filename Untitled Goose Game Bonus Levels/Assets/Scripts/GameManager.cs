using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
Event messaging system for Unity with flexible parameter passing
Code is 99% from the untiy tutorial here: https://unity3d.com/learn/tutorials/topics/scripting/events-creating-simple-messaging-system
the other 1% is from this forum post: https://forum.unity.com/threads/messaging-system-passing-parameters-with-the-event.331284/
*/


/* To use the event manager, the object that is listening for an event needs this information:
 * private UnityAction<string> EventListener;
 * 
 * private void Awake(){
 * EventListener = new UnityAction<string>(FunctionThatIsTheListener);
 * }
 *
 * private void OnEnable(){
 * EventManager.StartListening("EventName", FunctionThatIsTheListener);
 * }
 *
 * private void OnDisable()
 * {
 * EventManager.StopListening("EventName", FunctionThatIsTheListener);
 * }
 * 
 * void WhateverListeningFunctionIs(string JsonPassed) {
 *          ScriptableObjName newObj = ScriptableObject.CreateInstance<ScriptableObjName>();
 *          JsonUtility.FromJsonOverwrite(JsonPassed, newObj);       
 *          // Now you can access all variables passed inside of newObj by using newObj.variable
 *}

 * For an object that is sending the event, it needs this information:
 * private ScriptableObjName objRef;
 *
 * InsideSomeFunction(){
 *          objRef = ScriptableObject.CreateInstance<ScriptableObjName>();
 *          objRef.Variable = whateverInformation;
 *          string jsonVar = JsonUtility.ToJson(ammoVars);
 *          EventManager.TriggerEvent("UpdateAmmo", jsonVar);
 * }
 */

/// <summary>
///
/// Wrapper for unity events to serialize json with it to send data with the events to callback functions
/// 
/// </summary>
[System.Serializable]
public class ThisEvent : UnityEvent<string> { }

/// <summary>
///
/// Holds information to all events and their callbacks to decouple as much as possible in the game
/// 
/// </summary>
public class GameManager : MonoBehaviour
{
    // dictionary that holds the event name and the event data
    private Dictionary<string, ThisEvent> eventDictionary;

    // singleton instance of the event manager
    private static GameManager gameManager;

    // grabs the instance of the event manager
    /// <summary>
    ///
    /// Singleton instance to the event manager (Fix the singelton to delete other instances if they exist)
    /// 
    /// </summary>
    public static GameManager Instance
    {
        // getter method
        get
        {
            // if no event manager exists, then find it
            if (!gameManager)
            {
                gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

                // if it can not find it, then specify that an object in the scene needs to have it
                if (!gameManager)
                {
                    Debug.Log("There needs to be one active EventManager script on a gameobject in your scene.");
                }
                else
                {
                    // if not, then init the event manager
                    gameManager.Init();

                }
            }
            return gameManager;
        }
    }

    /// <summary>
    ///
    /// Initialize the event dictionary if it does not exist
    /// 
    /// </summary>
    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, ThisEvent>();
        }
    }

    /// <summary>
    ///
    /// Create an event instance by listening with specified callback function
    /// 
    /// </summary>
    /// <param name="eventName">String value of the event name</param>
    /// <param name="listener">Function callback for the event</param>
    public static void StartListening(string eventName, UnityAction<string> listener)
    {
        // init the event
        ThisEvent thisEvent = null;

        // if the dictionary is null, init it
        if (Instance.eventDictionary == null)
        {
            Instance.Init();
        }

        // if the event exists, then add the listener to this specific event to the dictionary
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            // if it does not exist, create it, then add it
            thisEvent = new ThisEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    /// <summary>
    ///
    /// Removes a callback tied to the specified event in the dictionary when an
    /// object is destroyed, removed or does not need the event data any longer
    /// 
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public static void StopListening(string eventName, UnityAction<string> listener)
    {
        // if the event dictionary is not active, return
        if (gameManager == null) return;

        // init the event
        ThisEvent thisEvent = null;

        // if the event exists, then remove it
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    /// <summary>
    ///
    /// Event trigger with specified event name and json data to be sent
    /// 
    /// </summary>
    /// <param name="eventName">Name of the event to trigger</param>
    /// <param name="json">Json data to send along with the event</param>
    public static void TriggerEvent(string eventName, string json)
    {
        // init event
        ThisEvent thisEvent = null;

        // if this event exists, then involke the event to all callback functions
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            // finally passes the message.
            thisEvent.Invoke(json);
        }
    }

    /// <summary>
    ///
    /// Event trigger with event name with no parameters
    /// 
    /// </summary>
    /// <param name="eventName"></param>
    public static void TriggerEvent(string eventName)
    {
        // init the event
        ThisEvent thisEvent = null;

        // see if this event is in the dictionary and if it is, involke it with no parameters
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            // pass the param as null as there is no variable to pass
            thisEvent.Invoke(null);
        }
    }
}