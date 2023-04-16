using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Modules : MonoBehaviour {

    //public static bool versionExtra = true;//neu true thi lam cho phien ban mo rong (tro choi moi)
    public static int versionCode = 2;//danh dau phien ban code de tranh bi loi save map
    //cac bien luu tru trong game
    public static int totalKey = 10;//de hoi sinh (co duoc trong luc choi hoac mua)
    public static int totalCoin = 0;//de mua do, nang cap do (co duoc trong luc choi hoac mua)
    public static float totalScore = 100;//diem ky luc cua nguoi choi
	public static float totalScoreNow = 0;//xu ly diem ky luc cua ban be
    public static int totalSkis = 5;//de lam van truot trong game
    public static int totalScoreBooster = 0;//de thuc cong diem nhan man choi chu dong
    public static int totalHeadStart = 0;//de thuc hien bay rocket chu dong
    public static int totalMysteryBox = 0;//tong hop trong game
    //khai bao list nhan vat
    public static List<GameObject> listHeroUse = new List<GameObject>();
    public static List<GameObject> listSkisUse = new List<GameObject>();
    public static GameObject facebookController;
    //cac bien xu ly trong man choi
    public static float speedGame = 0.78f;//thay doi toan bo speed game, animation
    public static float maxSpeedGame = 1.5f;//toc do toi da cho phep
    public static float moreSpeedAni = 0;//toc do them luc ban dau cho cac animation chay
    public static float maxSpeedAni = 1.75f;//toc do toi da cua cac animation run
    public static int timePlayer = 0;
    public static int coinPlayer = 0;//so coin trong man choi
    public static float scorePlayer = 0;//so diem trong man choi
    public static int xPointPlayer = 1;//so diem nhan trong man choi
    public static int numberXpoint = 1;//so da nhan de khi het thoi gian thi tru di
    public static int timeSaveMe = 0;//so lan hoi sinh de tinh key = 2^time
    //de thuc hien mo ra cac mon qua nhu tien, cac do de ghep
    public static bool showScorePlay = false;//de xu ly show diem ky luc hay diem man choi
    public static bool autoTapPlay = false;//xu ly tu dong chay khi load scene play
    public static string nextPageOpenBox = "ShowAchievement";
    //danh dau cac item dang su dung
    public static bool useSkis = false;//neu trang bi van truot
    public static bool usePower = false;//neu trang bi nhay cao
    public static bool useMagnet = false;//neu trang bi nam cham
    public static bool useRocket = false;//neu trang bi ten lua
    public static bool useJumper = false;//neu trang bi jumper
    public static bool useXPoint = false;//neu trang bi xPoint
    public static bool useCable = false;//neu trang bi cap treo
    public static bool useBonus = false;//neu dang su dung bonus road
    //thoi gian ton tai cac item dac biet
    public static Vector2 timeUseSkis = new Vector2(10, 12);
    public static Vector2 timeUsePower = new Vector2(5, 10);
    public static Vector2 timeUseMagnet = new Vector2(5, 10);
    public static Vector2 timeUseRocket = new Vector2(5, 10);
    public static Vector2 timeUseXPoint = new Vector2(5, 10);
    public static Vector2 timeUseCable = new Vector2(5, 10);
    public static int levelUpgradePower = 0, levelUpgradeMagnet = 0, levelUpgradeRocket = 0, levelUpgradeXPoint = 0, levelUpgradeCable = 0, levelUpgradeSkis = 0;
    public static int maxLevelItem = 10;
    public static float timeAddPerLevel = 3f;
    public static bool allowUseHoverbike = true;//dung de bat/tat chuc nang su dung hoverbike
    public static int maxHoverboard = 9999;
    public static int maxHeadstart = 10;
    public static int maxScorebooster = 7;
    //cac doi tuong luan chuyen trong game
    public static GameObject mainCharacter;
    public static GameObject itemSkis, itemShoeLeft, itemShoeRight, itemMagnet, itemRocket, itemCable;
    public static Text textCoinPlay, textScorePlay, textXPointPlay, textHighScore;
    public static GameObject panelShowEatItemsLeft, panelShowEatItemsRight, panelEffectAddPoint, panelHighScoreNow, panelViewEnemy, panelGameGuide;
    public static GameObject panelMissions, panelChallenge, panelBonus, panelCrackGlass;
    public static GameObject containMesItems, containButtonBuy, containEffectAddPoint;
    public static GameObject mesSaveMeBox, mesNotEnoughKey, bonusFirstBox, resultOnlineBox;
    public static GameObject panelBGEffectBonus, panelTextEffectBonus, panelTextEffectWinLose;
    public static GameObject parentResultOnline, panelFakeCity;
    //cac bien su dung trong man choi chinh (chay)
    public static StatusGame statusGame = StatusGame.menu;
    public static bool startBonusRoad = false;//neu bang true moi bat dau chay tren bonus, neu khong thi danh cho hieu ung chay nguoc map
    public static string codeHeroUse = "001";//mac dinh la loai nhan vat nay
    public static string codeSkisUse = "001";//mac dinh la loai skis nay
    public static int distanceEnemy = 1;//2 khuat, 1 nhin thay, 0 bi bat
    public static int timeFarEnemy = 5;//cu chay deu lau hon thoi gian nay thi enemy bi xa ra
    public static float speedSlowCollider = 0f;//chi so toc do bi cham lai khi va cham
    public static float speedAddUseRocket = 2f;//chi so toc do tang len khi su dung rocket, cap treo
    public static float speedAddMoreUse = 1f;//speed dang duoc su dung lam cham, nhanh them tu cac item
    public static List<string> listHeroUnlock = new List<string>() { "001" };
    public static List<string> listSkisUnlock = new List<string>() { "001" };
    public static float rangeReborn = 100f;//ban kinh pha huy cac vat can, items
    public static float rangeRunObj = 100f;//khoang cach chay xe bus, train...
    public static float rangeShowCoin = 50f;
    public static float rangeTakeOff = 30f;//khoang cach tao cac item, barrier khi ha canh boolone hoac road bonus
    public static float rangeHireBack = 10f;//khang cach them khi vat can va item ve phia sau thi an di cho nhe game
    public static float timeShowChest = 1.5f;
    public static bool runAffterDownBonus = false;//chi co tac dung phan chay ngay lap tuc sau khi roi tu bonus xuong
    public static bool runGameOverEffect = false;
    public static bool getPointFirst = false;
    //cac bien luu tru setting
    public static int indexLanguage = 0;
    public static int volumeBackground = 1;//1 true, 0 false
    public static int volumeAction = 1;//1 true, 0 false
    public static int reducedEffect = 0;//0 high, 1 medium, 2 low
    public static int skyEffect = 0;//1 true, 0 false
    public static int valueSensitivity = 60;//0=>100
    //public static int valueSpeedJumpUp = 14;//0=>40
    public static int countryEnemy = 1;//1 country, 0 friend
    public static int curvedWorld = 1;//1 true, 0 false
    public static int clickRate = 0;//1 la roi, 0 la chua
    public static int totalUseGame = 0;
    public static int autoCheckLang = 1;//0 thi khong check nua
    public static int indexTypeTerrain = 0;//vi tri the loai dia hinh dang chay
    public static int indexRunTerrain = 0;//vi tri dia hinh dang chay
    public static float pointRunTerrain = 0;//vi tri z hinh dang chay
    public static float pointHeroTerrain = 0;//vi tri cua hero so voi dia hinh (tren, giua, duoi)
    public static string listCodeTerrain = "";//ma terrain dang chay duoc luu lai
    //xu ly facebook sdk
    public static string fbID = "Null";//lay id facebook
	public static string fbName = "Null";
	public static string fbLinkAvatar = "Null";
	public static Sprite fbMyAvatar, fbEnemyAvatar;
    public static List<string> fbAvatarEnemy = new List<string>();//danh sach link avatar doi thu co diem so cao hon
    public static List<string> fbNameEnemy = new List<string>();//danh sach name doi thu co diem so cao hon
	public static List<float> fbHighScore = new List<float>();//danh sach diem so cua doi thu co diem so cao hon
    public static string bonusFacebook = "No";//neu bang 1 thi da thuong roi
    public static Color colorListLine = new Color(235f / 255f, 235f / 255f, 235f / 255f, 1f);
    public static int coinBonusInvite = 300;//so coin thuong sau moi lan moi 1 ban be
    public static int coinMaxInvite = 5000;//so coin thuong toi da, ke ca co moi nhieu hon nua
    //cac bien rank
    public static string linkPost = "http://baimathuat.ga/PostScoreBS.php";
    public static string linkGet = "http://baimathuat.ga/GetScoreBS.php";
    public static string linkGetCountry = "http://baimathuat.ga/GetScoreCountryBS.php";
    public static string linkGetDataMultiplayer = "http://baimathuat.ga/GetDataMultiplayerBS.php";
    public static string linkGetMissions = "http://baimathuat.ga/GetMissions.php";
    public static string linkGetChallenge = "http://baimathuat.ga/GetChallenge.php";
    public static float requestTime = 0.1f;
    public static float maxTime = 10f;//cho phep thuc hien toi da 10 giay
    //xu ly inApp purcharse va quang cao
    public static List<string> listProductID = new List<string>() { "ProductCoinA", "ProductKeyA" };
    public static string itemBonusViewAds = "Skis";
    public static Vector2 numberSkisBonus = new Vector2(1, 3);
    public static Vector2 numberKeyBonus = new Vector2(1, 2);
    public static int timeShowInterstitial = 2;//sau 5 lan chet moi hien thi quang cao
    public static int runTimeShowInterstitial = 0;
    //khai bao cac list icon
    public static List<Sprite> listIconItem = new List<Sprite>();//0 skis, 1 giay, 2 magnet, 3 rocket, 4 2X, 5 cable
    public static List<Sprite> listIconBonus = new List<Sprite>();//0 coin, 1 keys, 2 skis, 3 headStart, 4 scoreBooster
    public static List<ListMissionsClass> listMissions = new List<ListMissionsClass>();//chua cac icon, model de suu tap item
    public static List<ListChallengeClass> listChallenge = new List<ListChallengeClass>();//chua cac gia tri, model de suu tap chu
    public static Sprite iconHeadStart, iconScoreBooster, iconAvatarNull;
    //xu ly effect sang toi
    public static Vector2 KTCScenes = new Vector2(480, 800);
    //xu ly missions va challenge
    public static bool newMissions = false;
    public static bool newChallenge = false;
    public static string dataMissionsUse = "";//xu ly van de nhiem vu dang nhan
    public static string dataChallengeUse = "";//xu ly van de thu thach dang nhan
    public static string dataMissionsNew = "";//xu ly van de nhiem vu dang co san
    public static string dataChallengeNew = "";//xu ly van de thu thach dang co san
    public static string dataMissionsOld = "";//xu ly van de check nhiem vu da lam truoc do
    public static string dataChallengeOld = "";//xu ly van de check thu thach da lam truoc do
    public static int indexItemMissions = 0;
    public static int totalItemMissions = 0;
    public static int runItemMissions = 0;
    public static int indexBonusMissions = 0;
    public static int totalBonusMissions = 0;
    public static List<string> listTextRequire = new List<string>();
    public static List<string> listTextColect = new List<string>();
    public static int indexBonusChallenge = 0;
    public static int totalBonusChallenge = 0;
    public static bool autoGetMissions = true;//tu dong nhan neu chua co nhiem vu
    public static bool autoGetChallenge = true;//tu dong nhan neu chua co thu thach
    //xu ly gop cac scene
    public static GameObject containMainGame, containHeroConstruct, containAchievement, containShopItem, containHighScore, containOpenBox;
    public static GameObject listResources;//doi tuong chua cac component resources
    public static GameObject poolTerrains;//chua cac dia hinh
    public static GameObject poolOthers;//chua cac items, paticles
    //xu ly phan huong dan choi
    public static string gameGuide = "YES";
    public static int stepGuide = 0;//0 left, 1 right, 2 jump, 3 down, 4 double tap
    //them cac audios
    public static List<AudioSource> audioSource;
    public static AudioClip audioBackgrond, audioRoadBonus;
    public static AudioClip audioButton, audioTapPlay, audioOpenBox, audioSwipeMove, audioSwipeUp, audioSwipeDown, audioUpSkis;
    public static AudioClip audioCollider, audioColliderDie, audioSurprise, audioShowMessage, audioRocket, audioCable, audioTrapoline, audioBrokenGlass;
    public static AudioClip audioParReborn, audioParColSkis, audioBonusText, audioEatBonusBox;
    public static AudioClip audioPoStart, audioPoNear, audioPoFar;
    public static float maxPitchCoin = 0.95f;
    public static float minPitchCoin = 0.75f;
    public static float pitchCoin = 0.95f;
    public static float numChangePith = 0.04f;
    public static float oldTime = 0;
    //them cac particles
    public static GameObject parReborn, parSkisCollider, parSpeedFly;
    public static Text textDebug;
    //xu ly online mode
    public static string namePlayOnline = "";
    public static string nameRoomOnline = "";
    public static List<string> listNamePlayer = new List<string>();
    public static bool startViewOnline = false;
    public static GameObject networkManager;
    public static int winNow = 0, loseNow = 0, failNow = 0;
    //xu ly cac thuong them
    public static int bonusFirst = 0;//0 la chua, 1 la thuong roi
    public static int bonusWin = 0;//0 la chua, 1 la lan 1, 2 la lan 2, 3 la lan 3
    public static int totalWin = 0;//tong so lan win de tinh toan thuong

    public static void ResetGame()
    {
        statusGame = StatusGame.menu;
        startBonusRoad = false;
        speedGame = 0.78f;
        speedAddMoreUse = 1f;
        timePlayer = 0;
        coinPlayer = 0;
        scorePlayer = 0;
        xPointPlayer = 1;
        numberXpoint = 1;
        useSkis = false;
        usePower = false;
        useMagnet = false;
        useRocket = false;
        useJumper = false;
        useXPoint = false;
        useCable = false;
        useBonus = false;
        allowUseHoverbike = true;
        distanceEnemy = 1;
        timeSaveMe = 0;
        totalMysteryBox = 0;
        pitchCoin = 0.95f;
        oldTime = 0;
        showScorePlay = false;
        runGameOverEffect = false;
        getPointFirst = false;
        ResetMissions();
        ResetChallenge();
        if (panelCrackGlass) panelCrackGlass.gameObject.SetActive(false);
        panelHighScoreNow.SetActive(false);
        panelViewEnemy.SetActive(false);
        panelBonus.SetActive(false);
        panelBGEffectBonus.SetActive(false);
        panelTextEffectBonus.SetActive(false);
        panelTextEffectWinLose.SetActive(false);
        parentResultOnline.SetActive(false);
        if (gameGuide == "YES")
        {
            stepGuide = 0;
            foreach (Transform tran in panelGameGuide.transform) tran.gameObject.SetActive(true);
            Transform textGuide = panelGameGuide.transform.Find("TextGuide");
            textGuide.GetComponent<Text>().text = "Swipe left";
            Transform arrowGuide = panelGameGuide.transform.Find("ArrowGuide");
            arrowGuide.transform.eulerAngles = new Vector3(0, 0, 0);
            Transform iconItemBuy = panelGameGuide.transform.Find("IconItemBuy");
            iconItemBuy.GetComponent<Image>().enabled = false;
        }
        foreach (Transform tran in containButtonBuy.transform) Destroy(tran.gameObject);
        foreach (Transform tran in containMesItems.transform) Destroy(tran.gameObject);
        textXPointPlay.text = "x" + xPointPlayer;
        Modules.startViewOnline = false;
    }

    public static void ResetMissions()
    {
        if (dataMissionsUse == "") return;
        string[] data = dataMissionsUse.Split(';');
        indexItemMissions = IntParseFast(data[2]);
        totalItemMissions = IntParseFast(data[3]);
        indexBonusMissions = IntParseFast(data[4]);
        totalBonusMissions = IntParseFast(data[5]);
        runItemMissions = 0;
    }

    public static void ResetChallenge()
    {
        if (dataChallengeUse == "") return;
        listTextRequire = new List<string>();
        listTextColect = new List<string>();
        string[] data = dataChallengeUse.Split(';');
        string dataText = data[2];
        for (int i = 0; i < dataText.Length; i++)
            listTextRequire.Add(dataText.Substring(i, 1));
        indexBonusChallenge = IntParseFast(data[3]);
        string[] dataValue = data[4].Split(',');
        totalBonusChallenge = UnityEngine.Random.Range(IntParseFast(dataValue[0]), IntParseFast(dataValue[1]));
    }

    public static void RebornGame()
    {
        statusGame = StatusGame.play;
        speedAddMoreUse = 1f;
        useSkis = false;
        usePower = false;
        useMagnet = false;
        useRocket = false;
        useJumper = false;
        useXPoint = false;
        useCable = false;
        useBonus = false;
        distanceEnemy = 2;
        timeSaveMe++;
        if (panelCrackGlass) panelCrackGlass.gameObject.SetActive(false);
        runGameOverEffect = false;
        getPointFirst = false;
    }

    public static Vector2 GetTimeItemUse(TypeItems codeItem)
    {
        if (codeItem == TypeItems.sneaker)//power
            return new Vector2(timeUsePower.x + timeAddPerLevel * levelUpgradePower, timeUsePower.y + timeAddPerLevel * levelUpgradePower);
        else if (codeItem == TypeItems.magnet)//magnet
            return new Vector2(timeUseMagnet.x + timeAddPerLevel * levelUpgradeMagnet, timeUseMagnet.y + timeAddPerLevel * levelUpgradeMagnet);
        else if (codeItem == TypeItems.jetpack)//rocket
            return new Vector2(timeUseRocket.x + timeAddPerLevel * levelUpgradeRocket, timeUseRocket.y + timeAddPerLevel * levelUpgradeRocket);
        else if (codeItem == TypeItems.xpoint)//x point
            return new Vector2(timeUseXPoint.x + timeAddPerLevel * levelUpgradeXPoint, timeUseXPoint.y + timeAddPerLevel * levelUpgradeXPoint);
        else if (codeItem == TypeItems.hoverbike)//cable
            return new Vector2(timeUseCable.x + timeAddPerLevel * levelUpgradeCable, timeUseCable.y + timeAddPerLevel * levelUpgradeCable);
        else
            return new Vector2(timeUseSkis.x + timeAddPerLevel * levelUpgradeSkis, timeUseSkis.y + timeAddPerLevel * levelUpgradeSkis);
    }

    public static int SecondsToTimePerFrame(int seconds)
    {
        return Mathf.CeilToInt(seconds / Time.fixedDeltaTime);
    }

    public static int IntParseFast(string value)
    {
        int result = 0;
        for (int i = 0; i < value.Length; i++)
        {
            char letter = value[i];
            result = 10 * result + (letter - 48);
        }
        return result;
    }

    public static GameObject GetChildGameObject(GameObject obj, string name)
    {
        GameObject ketQua = null;
        foreach (Transform go in obj.transform)
        {
            if (go.name == name)
            {
                ketQua = go.gameObject;
                break;
            }
            else if (go.childCount > 0)
            {
                ketQua = GetChildGameObject(go.gameObject, name);
                if (ketQua != null)
                    break;
            }
        }
        return ketQua;
    }

    public static void RemoveModelUseItem(Transform tran, string nameItem)
    {
        GameObject goAddItem = Modules.GetChildGameObject(tran.gameObject, nameItem);
        if (goAddItem != null)
            Destroy(goAddItem);
    }

    public static GameObject HideModelUseItem(Transform tran, string nameItem)
    {
        GameObject goAddItem = Modules.GetChildGameObject(tran.gameObject, nameItem);
        if (goAddItem != null)
            goAddItem.SetActive(false);
        return goAddItem;
    }

    public static void SetAllRenderer(Transform tran, List<string> nameStop, bool status)
    {
        if (tran.GetComponent<Renderer>() != null)
        {
            tran.GetComponent<Renderer>().enabled = status;
        }
        foreach (Transform child in tran)
        {
            if (!nameStop.Contains(child.name))
                SetAllRenderer(child, nameStop, status);
        }
    }

    public static void ShowPanelEffectAddPoint(int pointAdd, Vector3 location, GameObject parent, float timeDestroy)
    {
        GameObject panelShow = Instantiate(panelEffectAddPoint, location, Quaternion.identity) as GameObject;
        panelShow.transform.SetParent(parent.transform, false);
        Transform number = panelShow.transform.Find("TextXPoint");
        Text txtNumber = number.GetComponent<Text>();
        txtNumber.text = "+" + pointAdd.ToString();
        Destroy(panelShow, timeDestroy);
    }

    public static void UpdateValueMissions()
    {
        Transform tranIcon = panelMissions.transform.Find("IconMissions");
        Transform tranValueA = panelMissions.transform.Find("TextMissions");
        Transform tranValue = tranValueA.transform.Find("TextMissions");
        Image imgIcon = tranIcon.GetComponent<Image>();
        Text txtValue = tranValue.GetComponent<Text>();
        imgIcon.sprite = listMissions[indexItemMissions].icon;
        txtValue.text = runItemMissions + "/" + totalItemMissions;
    }

    public static void UpdateValueChallenge()
    {
        Transform tranContent = panelChallenge.transform.Find("Content");
        foreach (Transform tran in tranContent) if (tran.name != "Text") Destroy(tran.gameObject);
        Transform tranText = tranContent.Find("Text");
        for (int i = 0; i < listTextRequire.Count; i++)
        {
            GameObject textNew = tranText.gameObject;
            if (i > 0)
                textNew = Instantiate(tranText.gameObject, tranContent) as GameObject;
            Text txtValue = textNew.GetComponent<Text>();
            txtValue.text = listTextRequire[i];
            Color setColor = new Color(0, 40f / 255f, 70f / 255f, 1f);
            if (i < listTextColect.Count)
                setColor = Color.yellow;
            txtValue.color = setColor;
        }
    }

    private static int oldTypeMessage = 2;
    
    public static DateTime LoadOldDateTime(string firstSave)
    {
        DateTime dateTimeNow = DateTime.Now;
        int oldYear = PlayerPrefs.GetInt(firstSave + "Year", dateTimeNow.Year);
        int oldMonth = PlayerPrefs.GetInt(firstSave + "Month", dateTimeNow.Month);
        int oldDay = PlayerPrefs.GetInt(firstSave + "Day", dateTimeNow.Day);
        int oldHour = PlayerPrefs.GetInt(firstSave + "Hour", dateTimeNow.Hour);
        int oldMinute = PlayerPrefs.GetInt(firstSave + "Minute", dateTimeNow.Minute);
        int oldSecond = PlayerPrefs.GetInt(firstSave + "Second", dateTimeNow.Second);
        return new DateTime(oldYear, oldMonth, oldDay, oldHour, oldMinute, oldSecond);
    }

    public static void SaveNewDateTime(string firstSave)
    {
        DateTime dateTimeNow = DateTime.Now;
        PlayerPrefs.SetInt(firstSave + "Year", dateTimeNow.Year);
        PlayerPrefs.SetInt(firstSave + "Month", dateTimeNow.Month);
        PlayerPrefs.SetInt(firstSave + "Day", dateTimeNow.Day);
        PlayerPrefs.SetInt(firstSave + "Hour", dateTimeNow.Hour);
        PlayerPrefs.SetInt(firstSave + "Minute", dateTimeNow.Minute);
        PlayerPrefs.SetInt(firstSave + "Second", dateTimeNow.Second);
        PlayerPrefs.Save();
    }

    private static float oldTimePlayCoin = 0;//xu ly dan cach am thanh, khong cho phep chay am lien tuc day rit
    public static void PlayAudioClipFree(AudioClip audio, bool typeCoin = false)
    {
        if (audio != null && volumeAction > 0)
        {
            if (typeCoin)
            {
                if (Time.time - oldTimePlayCoin < 0.1f) return;
                if (oldTime == 0)
                {
                    oldTime = Time.time;
                    pitchCoin = minPitchCoin;
                }
                else
                {
                    if (Time.time - oldTime < 0.5f)
                    {
                        oldTime = Time.time;
                        pitchCoin += numChangePith;
                        if (pitchCoin > maxPitchCoin) pitchCoin = maxPitchCoin;
                    }
                    else
                    {
                        oldTime = Time.time;
                        pitchCoin = minPitchCoin;
                    }
                }
            }
            //AudioSource.PlayClipAtPoint(audio, Camera.main.transform.position, volumeAction);
            int indexPlay = 0;
            for (int i = 0; i < audioSource.Count; i++)
            {
                if (!audioSource[i].isPlaying) { indexPlay = i; break; }
            }
            if (indexPlay < audioSource.Count)
            {
                audioSource[indexPlay].clip = audio;
                if (!typeCoin)
                    audioSource[indexPlay].pitch = 1f;
                else
                {
                    audioSource[indexPlay].pitch = pitchCoin;
                    oldTimePlayCoin = Time.time;
                }
                audioSource[indexPlay].Play();
            }
        }
    }

    public static void PlayAudioClipLoop(AudioClip audio, Transform tran)
    {
        if (tran.GetComponent<AudioSource>() == null)
            tran.gameObject.AddComponent<AudioSource>();
        tran.GetComponent<AudioSource>().clip = audio;
        tran.GetComponent<AudioSource>().volume = volumeBackground;
        tran.GetComponent<AudioSource>().loop = true;
        tran.GetComponent<AudioSource>().Play();
    }

    public static void SetLayer(GameObject parent, string layerName, bool includeChildren = true)
    {
        parent.layer = LayerMask.NameToLayer(layerName);
        if (includeChildren)
            foreach (Transform trans in parent.transform)
                SetLayer(trans.gameObject, layerName, includeChildren);
    }

#if UNITY_IOS
    public static string linkStoreGame = "?index=1";
#else
    public static string linkStoreGame = "?index=0";
#endif
    public static string linkIconGame = "http://52.220.93.168/Others/Icons/IconFruntastic.png";
    public static string linkChange = "http://52.220.93.168/Others/ChangeLink.php";
    public static string linkShortFB = "https://fb.me/303070490147052";

}
[System.Serializable]//de show ra phan input cua unity editor
public class SetupItemBody
{
    public string codeBody;
    public string nameBoneAdd;
    public Vector3 localPoint;
    public Vector3 localAngle;
    public Vector3 localScale;
    public SetupItemBody(string codeBodyInput, string nameBoneAddInput, Vector3 localPointInput, Vector3 localAngleInput, Vector3 localScaleInput)
    {
        this.codeBody = codeBodyInput;
        this.nameBoneAdd = nameBoneAddInput;
        this.localPoint = localPointInput;
        this.localAngle = localAngleInput;
        this.localScale = localScaleInput;
    }
}

[System.Serializable]//de show ra phan input cua unity editor
public class ListCodeTerrain
{
    public List<string> listTerrain = new List<string>();
    public ListCodeTerrain(List<string> listTerrainInput)
    {
        this.listTerrain = listTerrainInput;
    }
}

[System.Serializable]//de show ra phan input cua unity editor
public class ListUseTerrain
{
    public List<GameObject> listTerrain = new List<GameObject>();
    public ListUseTerrain(List<GameObject> listTerrainInput)
    {
        this.listTerrain = listTerrainInput;
    }
}

[System.Serializable]//de show ra phan input cua unity editor
public class ListMissionsClass
{
    public Sprite icon;
    public GameObject model;
    public ListMissionsClass(Sprite iconInput, GameObject modelInput)
    {
        this.icon = iconInput;
        this.model = modelInput;
    }
}

[System.Serializable]//de show ra phan input cua unity editor
public class ListChallengeClass
{
    public string value;
    public GameObject model;
    public ListChallengeClass(string valueInput, GameObject modelInput)
    {
        this.value = valueInput;
        this.model = modelInput;
    }
}
public enum StatusGame
{
    menu,
    start,
    play,
    pause,
    flyScene,
    over,
    bonusEffect,
    stop
}

//CAC DIEU CAN CHU Y VE CAC LAYER
/*
MCG-Terrain => Danh cho cac Box khong lam nhan vat chet, xu ly va cham nhu dia hinh
MCG-Barrier => Danh cho cac Box co cac mat chet (front)
*/