using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMgr : BaseSingleton<CharacterMgr>
{
    public void CreateCharacter(string id, System.Action<CharacterBase> callback)
    {
        if (!TableMgr.Instance.ExistTableID<Tbl_Character>(id))
        {
            return;
        }

        Tbl_Character tbl_Character = TableMgr.Instance.GetTable<Tbl_Character>(id);
        AssetsLoadMgr.Instance.LoadAsync("role_jinglingnan", "Characters/Jinglingnan/Prefabs/JingLingNan.prefab", (string _name, UnityEngine.Object _obj) =>
        {
            GameObject player = GameObject.Instantiate(_obj as GameObject);
            player.name = tbl_Character.Name;
            player.transform.position = Vector3.zero;
            player.transform.localScale = Vector3.one;

            CharacterMain character = player.AddComponent<CharacterMain>();
            character.characterData.name = tbl_Character.Name;
            character.characterData.moveSpeed = tbl_Character.MoveSpeed;
            character.characterData.hp = tbl_Character.Hp;
            character.characterData.attack = tbl_Character.Attack;
            character.characterData.defence = tbl_Character.Defence;
            character.characterData.describe = tbl_Character.Describe;

            callback?.Invoke(character);
        });
    }
}
