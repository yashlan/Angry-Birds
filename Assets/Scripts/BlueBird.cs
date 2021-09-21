using UnityEngine;
using Yashlan.bird;

public class BlueBird : Bird
{
    [SerializeField]
    private float _boostForce;
    [SerializeField]
    private bool _hasSpawned = false;

    private void Spawn()
    {
        if (State == BirdState.Thrown && !_hasSpawned)
        {
            //lanjot besok 
            _hasSpawned = true;
            Instantiate(SmokeEffect, transform.position, Quaternion.identity);
            for (int i = 0; i < 2; i++)
            {
                var pos = transform.position + new Vector3(transform.position.x, Random.Range(0, 5), transform.position.z);
                var clone = Instantiate(gameObject, pos, Quaternion.identity);
                clone.GetComponent<Bird>().State = BirdState.Thrown;
                var rbClone = clone.GetComponent<Rigidbody2D>();
                rbClone.GetComponent<CircleCollider2D>().enabled = true;
                rbClone.bodyType = RigidbodyType2D.Dynamic;
                rbClone.AddForce(Rigidbody.velocity * _boostForce);
            }
            print("kagebunshin no jutsu");
        }
    }

    public override void OnTap() => Spawn();
}
