namespace ProjectW.Define
{
    /// <summary>
    /// 게임에 사용되는 씬 종류
    /// </summary>
    public enum SceneType 
    {
        Title, 
        Loading,
        StartingVillage,
        BeginnerHuntingGround,
    }

    /// <summary>
    /// 타이틀 씬에서 순차적으로 수행할 작업
    /// </summary>
    public enum IntroPhase
    {
        Start,
        AppSetting,
        Server,
        StaticData,
        UserData,
        Resource,
        UI,
        Complete
    }

    public class Input
    {
        public const string AxisX = "Horizontal";
        public const string AxisZ = "Vertical";
        public const string MouseX = "Mouse X";
        public const string MouseY = "Mouse Y";
        public const string FrontCam = "Fire3";
        public const string Jump = "Jump";
        public const string MouseLeft = "Fire1";
        public const string MouseRight = "Fire2";
    }

    public class UI
    {
        public enum UIType { IngameUI, }
    }

    public class Camera
    {
        public enum View { Default, Front }
        public const float RotSpeed = 2f;
        public const string CamPosPath = "Prefabs/CamPos";
    }

    public class Actor
    {
        /// <summary>
        /// 액터의 종류
        /// </summary>
        public enum Type { Character, Monster }

        /// <summary>
        /// 액터의 일반공격 타입
        /// Normal 근접공격, Projectile 발사체를 이용한 공격
        /// </summary>
        public enum AttackType { Normal, Projectile }

        /// <summary>
        /// 액터의 상태
        /// </summary>
        public enum State { None, Idle, Walk, Jump, Attack, Hit, Die }

        public enum AnimParameter { IsWalk, IsJump, AttackTrigger, IsHit }
    }

    public class Effect
    {
        public enum Monster { Monster_Spawn, Monster_Hit, Monster_Despawn }
    
    }

    public class Item
    {
        public enum Type { Equip, Consume, Material }

        public enum SubType { HpPotion, MpPotion, }
    }

    public class StaticData
    {
        public const string SDPath = "Assets/StaticData";
        public const string SDExcelPath = "Assets/StaticData/Excel";
        public const string SDJsonPath = "Assets/StaticData/Json";
    }
}
