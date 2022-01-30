using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitParticleControl : MonoBehaviour
{
    [SerializeField] GameObject linkParticlePrefab;
    [SerializeField] ParticleSystem bounceParticlePrefab;
    Dictionary<Vector2Int, GameObject> LinkParticles;
    Dictionary<BaseUnit, GameObject> LinkParticlesUnit;
    BaseUnit unit;
    // Start is called before the first frame update
    public void Init(BaseUnit _unit)
    {
        unit = _unit;
        InitParticle();
        InitDict();
        unit.OnBind += ShowBondParticle;
        unit.OnUnBind += HideBondParticle;
        unit.OnPush += ShowBounceParticle;

    }

    private void InitParticle()
    {
        if(unit.Type == BaseUnit.UnitType.RED)
        {
            if(linkParticlePrefab == null) Resources.Load<GameObject>("Particle/Linkparticle_Red");
            if (bounceParticlePrefab == null) bounceParticlePrefab = InitParticle("BouneParticle_Red Variant");

        }
        else if (unit.Type == BaseUnit.UnitType.BLUE)
        {
            if (linkParticlePrefab == null) Resources.Load<GameObject>("Particle/Linkparticle_Blue Variant");
            if (bounceParticlePrefab == null) bounceParticlePrefab = InitParticle("BouneParticle_Blue Variant");
        }
    }

    private ParticleSystem InitParticle(string _name)
    {
        var _p= Resources.Load<ParticleSystem>($"Particle/{_name}");
        return Instantiate(_p, transform);
    }

    private void InitDict()
    {
        LinkParticlesUnit = new Dictionary<BaseUnit, GameObject>();
        LinkParticles = new Dictionary<Vector2Int, GameObject>();
        LinkParticles.Add(new Vector2Int(1, 0), Instantiate(linkParticlePrefab, transform.position, Quaternion.identity, transform));
        LinkParticles.Add(new Vector2Int(0, 1), Instantiate(linkParticlePrefab, transform.position, Quaternion.Euler(0, 0, 90), transform));
        LinkParticles.Add(new Vector2Int(-1, 0), Instantiate(linkParticlePrefab, transform.position, Quaternion.Euler(0, 0, 180), transform));
        LinkParticles.Add(new Vector2Int(0, -1), Instantiate(linkParticlePrefab, transform.position, Quaternion.Euler(0, 0, 270), transform));
        foreach (var _v in LinkParticles.Values)
        {
            _v.SetActive(false);
        }
    }

    public void ShowBondParticle(BaseUnit _unit)
    {
        var _dir = _unit.MyTile.Pos - unit.MyTile.Pos;
        LinkParticles[_dir].SetActive(true);
        LinkParticlesUnit.Add(_unit, LinkParticles[_dir]);
    }

    public void HideBondParticle(BaseUnit _unit)
    {
        LinkParticlesUnit[_unit].SetActive(false);
        LinkParticlesUnit.Remove(_unit);
    }

    public void ShowBounceParticle()
    {
        bounceParticlePrefab.Play();
    }
}