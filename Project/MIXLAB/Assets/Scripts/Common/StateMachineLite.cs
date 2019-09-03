using System.Collections.Generic;

public class StateMachineLite
{
    public delegate void FSMCallfunc(params object[] param);

    class FSMState
    {
        private string mName;
        public FSMCallfunc OnEnterStateCallback;
        public FSMCallfunc OnExitStateCallback;

        public FSMState(string name, FSMCallfunc enterCallback = null, FSMCallfunc exitCallback = null)
        {
            mName = name;
            OnEnterStateCallback = enterCallback;
            OnExitStateCallback = exitCallback;
        }

        public readonly Dictionary<string, FSMTranslation> TranslationDict = new Dictionary<string, FSMTranslation>();
    }

    class FSMTranslation
    {
        public string FromState;
        public string Name;
        public string ToState;
        public FSMCallfunc OnStateChangeCallback; // 回调函数

        public FSMTranslation(string fromState, string name, string toState, FSMCallfunc onStateChangeCallback)
        {
            FromState = fromState;
            ToState = toState;
            Name = name;
            OnStateChangeCallback = onStateChangeCallback;
        }
    }

    public string currentState { get; private set; }

    private readonly Dictionary<string, FSMState> mStateDict = new Dictionary<string, FSMState>();

    public void AddState(string stateName)
    {
        mStateDict[stateName] = new FSMState(stateName);
    }

    public void AddTranslation(string fromState, string actionName, string toState, FSMCallfunc callfunc = null)
    {
        mStateDict[fromState].TranslationDict[actionName] =
            new FSMTranslation(fromState, actionName, toState, callfunc);
    }

    public void Start(string stateName)
    {
        if (string.IsNullOrEmpty(stateName))
            return;

        currentState = stateName;
        if (mStateDict.ContainsKey(currentState) && mStateDict[currentState].OnEnterStateCallback != null)
        {
            mStateDict[currentState].OnEnterStateCallback();
        }
    }

    public void ChangeState(string actionName, params object[] param)
    {
        if (string.IsNullOrEmpty(currentState))
            return;

        if (mStateDict[currentState].TranslationDict.ContainsKey(actionName))
        {
            FSMTranslation tempTranslation = mStateDict[currentState].TranslationDict[actionName];

            if (mStateDict[tempTranslation.FromState].OnExitStateCallback != null)
            {
                mStateDict[tempTranslation.FromState].OnExitStateCallback();
            }

            if (tempTranslation.OnStateChangeCallback != null)
            {
                tempTranslation.OnStateChangeCallback(param);
            }

            currentState = tempTranslation.ToState;
            if (mStateDict[tempTranslation.ToState].OnEnterStateCallback != null)
            {
                mStateDict[tempTranslation.ToState].OnEnterStateCallback(tempTranslation.FromState);
            }
        }
    }

    public void ClearStateDict()
    {
        mStateDict.Clear();
    }

    public void ClearTranslationDict(string stateName = null)
    {
        if (string.IsNullOrEmpty(stateName))
        {
            foreach (var keyValuePair in mStateDict)
            {
                keyValuePair.Value.TranslationDict.Clear();
            }
        }
        else if (mStateDict[stateName] != null)
        {
            mStateDict[stateName].TranslationDict.Clear();
        }
    }
}