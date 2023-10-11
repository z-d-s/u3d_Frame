using UnityEngine;

public class CharacterMgr : BaseSingleton<CharacterMgr>
{
    /// <summary>
    /// 主角色
    /// </summary>
    public CharacterMain mainCharacter;

    #region 创建主角色
    public CharacterMain CreateCharacter(string id, int level)
    {
        if (!TableMgr.Instance.ExistTableID<Tbl_Character>(id))
        {
            return null;
        }

        Tbl_Character tbl_Character = TableMgr.Instance.GetTable<Tbl_Character>(id);
        string characterName = UtilTool.GetNameFromPath(tbl_Character.AssetNamePath);
        string rootCharacterName = "root_" + characterName;
        GameObject obj_Character = PoolMgr.Instance.GetObject(rootCharacterName);

        if (obj_Character == null)
        {
            obj_Character = new GameObject(rootCharacterName);

            CharacterMain character = obj_Character.AddComponent<CharacterMain>();
            character.characterBaseData.id = id;
            character.characterBaseData.name = tbl_Character.Name;
            character.characterBaseData.describe = tbl_Character.Describe;

            UnityEngine.Object _obj = AssetsLoadMgr.Instance.LoadSync(tbl_Character.AssetBundleName, tbl_Character.AssetNamePath);
            GameObject realCharacter = GameObject.Instantiate(_obj as GameObject);
            realCharacter.name = characterName;
            realCharacter.AddComponent<SmoothFollow>().SetTarget(obj_Character);

            character.SetRealCharacter(realCharacter);

            return character;
        }
        else
        {
            CharacterMain character = obj_Character.GetComponent<CharacterMain>();

            return character;
        }
    }

    public void CreateCharacter(string id, int level, System.Action<CharacterMain> callback)
    {
        if (!TableMgr.Instance.ExistTableID<Tbl_Character>(id))
        {
            callback?.Invoke(null);
            return;
        }

        Tbl_Character tbl_Character = TableMgr.Instance.GetTable<Tbl_Character>(id);
        string characterName = UtilTool.GetNameFromPath(tbl_Character.AssetNamePath);
        string rootCharacterName = "root_" + characterName;
        GameObject obj_Character = PoolMgr.Instance.GetObject(rootCharacterName);

        if (obj_Character == null)
        {
            obj_Character = new GameObject(rootCharacterName);
            CharacterMain character = obj_Character.AddComponent<CharacterMain>();
            character.characterBaseData.id = id;
            character.characterBaseData.name = tbl_Character.Name;
            character.characterBaseData.describe = tbl_Character.Describe;

            AssetsLoadMgr.Instance.LoadAsync(tbl_Character.AssetBundleName, tbl_Character.AssetNamePath, (string _assetName, UnityEngine.Object _obj) =>
            {
                GameObject realCharacter = GameObject.Instantiate(_obj as GameObject);
                realCharacter.name = characterName;
                realCharacter.AddComponent<SmoothFollow>().SetTarget(obj_Character);

                character.SetRealCharacter(realCharacter);
                callback?.Invoke(character);
            });
        }
        else
        {
            CharacterMain character = obj_Character.GetComponent<CharacterMain>();
            callback?.Invoke(character);
        }
    }
    #endregion

    #region 创建敌人
    public CharacterEnemy CreateEnemy(string id, int level)
    {
        if (!TableMgr.Instance.ExistTableID<Tbl_Enemy>(id))
        {
            return null;
        }

        Tbl_Enemy tbl_Enemy = TableMgr.Instance.GetTable<Tbl_Enemy>(id);
        string characterName = UtilTool.GetNameFromPath(tbl_Enemy.AssetNamePath);
        string rootCharacterName = "root_" + characterName;
        GameObject obj_Character = PoolMgr.Instance.GetObject(rootCharacterName);

        if (obj_Character == null)
        {
            obj_Character = new GameObject(rootCharacterName);
            CharacterEnemy enemy = obj_Character.AddComponent<CharacterEnemy>();
            enemy.characterBaseData.id = id;
            enemy.characterBaseData.name = tbl_Enemy.Name;
            enemy.characterBaseData.describe = tbl_Enemy.Describe;
            enemy.RefreshDataByLevel(level);

            UnityEngine.Object _obj = AssetsLoadMgr.Instance.LoadSync(tbl_Enemy.AssetBundleName, tbl_Enemy.AssetNamePath);
            GameObject realCharacter = GameObject.Instantiate(_obj as GameObject);
            realCharacter.name = characterName;
            realCharacter.AddComponent<SmoothFollow>().SetTarget(obj_Character);

            enemy.SetRealCharacter(realCharacter);

            return enemy;
        }
        else
        {
            CharacterEnemy enemy = obj_Character.GetComponent<CharacterEnemy>();
            enemy.RefreshDataByLevel(level);

            return enemy;
        }
    }

    public void CreateEnemy(string id, int level, System.Action<CharacterEnemy> callback)
    {
        if (!TableMgr.Instance.ExistTableID<Tbl_Enemy>(id))
        {
            callback?.Invoke(null);
            return;
        }

        Tbl_Enemy tbl_Enemy = TableMgr.Instance.GetTable<Tbl_Enemy>(id);
        string characterName = UtilTool.GetNameFromPath(tbl_Enemy.AssetNamePath);
        string rootCharacterName = "root_" + characterName;
        GameObject obj_Character = PoolMgr.Instance.GetObject(rootCharacterName);

        if (obj_Character == null)
        {
            obj_Character = new GameObject(rootCharacterName);
            CharacterEnemy enemy = obj_Character.AddComponent<CharacterEnemy>();
            enemy.characterBaseData.id = id;
            enemy.characterBaseData.name = tbl_Enemy.Name;
            enemy.characterBaseData.describe = tbl_Enemy.Describe;
            enemy.RefreshDataByLevel(level);

            AssetsLoadMgr.Instance.LoadAsync(tbl_Enemy.AssetBundleName, tbl_Enemy.AssetNamePath, (string _assetName, UnityEngine.Object _obj) =>
            {
                GameObject realCharacter = GameObject.Instantiate(_obj as GameObject);
                realCharacter.name = characterName;
                realCharacter.AddComponent<SmoothFollow>().SetTarget(obj_Character);

                enemy.SetRealCharacter(realCharacter);
                callback?.Invoke(enemy);
            });
        }
        else
        {
            CharacterEnemy enemy = obj_Character.GetComponent<CharacterEnemy>();
            enemy.RefreshDataByLevel(level);

            callback?.Invoke(enemy);
        }
    }
    #endregion
}
