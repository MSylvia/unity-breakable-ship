using System;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    protected string displayname;

    protected string type;

    protected float health;

    protected float maxHealth;

    protected float mass;

    protected Sprite[] sprites;

    public List<Part> weldedTo;

    public ShipContainer shipContainer;

    public ShipChunk shipChunk;

    public Rigidbody2D rigidbody2d;

    public float Health
    {
        get
        {
            return this.health;
        }
    }

    public string Name
    {
        get
        {
            return this.displayname;
        }
    }

    public string Type
    {
        get
        {
            return this.type;
        }
    }

    public Part()
    {
    }

    public void AddForce(Vector2 force)
    {
        this.rigidbody2d.AddForceAtPosition(force, base.transform.position);
    }

    public virtual void Damage(float amount, Part attacker)
    {
        if (amount > this.health)
            amount = this.health;
        
        this.shipContainer.PartDamaged(amount);
        this.health = Mathf.Clamp(this.health - amount, 0f, this.maxHealth);
        if (this.health <= 0f) {
            if (this.type != "core") {
                this.Kill();
            } else {
                this.shipContainer.KillShip();
            }
        }
    }

    public void Detach()
    {
        foreach (Part part in this.weldedTo)
            part.weldedTo.Remove(this);
        
        List<List<Part>> chunks = new List<List<Part>>();

        foreach (Part part in this.weldedTo) {
            bool found = false;
            foreach (List<Part> list in chunks) {
                if (!list.Contains(part))
                    continue;
                
                found = true;
                break;
            }

            if (found)
                continue;
            
            chunks.Add(ShipChunk.TraverseChunk(part, null));
        }

        this.InstantiateNewShipChunks(chunks);
        this.weldedTo.Clear();

        Debug.Log("Detached");
    }

    public void Heal(float amount)
    {
        if (amount > this.maxHealth - this.health)
            amount = this.maxHealth - this.health;
        
        this.shipContainer.PartDamaged(-amount);
        this.health = Mathf.Clamp(this.health + amount, 0f, this.maxHealth);
    }

    public virtual void Init(string name)
    {
        this.displayname = (name == null ? string.Empty : name);
        this.weldedTo = new List<Part>();
    }

    private void InstantiateNewShipChunks(List<List<Part>> chunks)
    {
        foreach (List<Part> chunk in chunks) {
            GameObject obj = Resources.Load<GameObject>("Chunk");
            Vector3 _position = chunk[0].transform.position;
            float single = _position.x;
            Vector3 vector3 = chunk[0].transform.position;

            ShipChunk component = ((GameObject)UnityEngine.Object.Instantiate(obj, new Vector3(single, vector3.y, 0f), Quaternion.identity)).GetComponent<ShipChunk>();
            component.SetShipContainer(this.shipContainer);

            Vector2 _zero = Vector2.zero;
            foreach (Part part in chunk) {
                _zero = _zero + this.rigidbody2d.GetPointVelocity(part.transform.position);
                part.SetShipChunk(component);
            }

            _zero = _zero / (float)chunk.Count;
            component.rigidbody2d.velocity = (_zero);
            component.rigidbody2d.angularVelocity = (this.rigidbody2d.angularVelocity);
        }
    }

    public void Kill()
    {
        this.shipContainer.parts.Remove(this);
        this.shipContainer.PartKilled();

        this.Detach();
        
        float single = 4f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(base.transform.position, single);
        for (int i = 0; i < (int)collider2DArray.Length; i++) {
            Part component = collider2DArray[i].GetComponent<Part>();
            if (component != null) {
                float single1 = Vector2.Distance(base.transform.position, component.transform.position);
                float single2 = single1 / single;
                Vector2 _position = component.transform.position - base.transform.position;
                Vector2 _normalized = 200f * single2 * _position.normalized;
                component.AddForce(_normalized);
            }
        }

        this.shipContainer.PartDamaged(this.health);
        if (this.shipChunk.transform.childCount != 1) {
            UnityEngine.Object.Destroy(base.gameObject);
		} else {
            UnityEngine.Object.Destroy(this.shipChunk.gameObject);
        }
    }

    private void OnDestroy()
    {
    }

    public void SetShipChunk(ShipChunk chunk)
    {
        this.shipChunk = chunk;
        base.transform.SetParent(chunk.transform);
        this.rigidbody2d = chunk.rigidbody2d;
        chunk.rigidbody2d.mass = (((double)chunk.rigidbody2d.mass >= 0.01 ? chunk.rigidbody2d.mass + this.mass : this.mass));
        this.shipContainer = chunk.shipContainer;
    }
}