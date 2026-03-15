using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RocketEngine
{
	public abstract class ComponentBase : Instantiable
	{

		private float x = 0;
		private float y = 0;

		private Vector2 forward = new Vector2(1, 0);
		private Vector2 up = new Vector2(0, -1);

		private float rotation = 0;

		public Vector2 Position { get { return GetPosition(); } set { SetPosition(value); } }


		private List<InstantiableComponent> components = new List<InstantiableComponent>();

		public List<InstantiableComponent> Components 
		{ 
			get 
			{ 
				return components; 
			} 
			private set 
			{ 
				components = value; 
			}
		}

		public Action<InstantiableComponent> onComponentAdded;
		public Action<InstantiableComponent> onComponentDestroyRequested;
		public Action<ComponentBase> onDestroyed;

		public Action<Vector2> onPositionChanged;
		public Action<float> onRotationChanged;
		public Action onTransformChanged; // gets called if pos or rotation changes

		public ComponentBase() : base()
		{
			this.x = 0;
			this.y = 0;
			Name = "ComponentBase";
		}

		public ComponentBase(float x = 0, float y = 0, string name = "ComponentBase") : base()
		{
			this.x = x;
			this.y = y;
			Name = name;
		}


		// I prefer this way of getting and setting positions over the propertys because I like working with vector 2 in this case

		public Vector2 GetPosition()
		{ return new Vector2(x, y); }

		public float GetPositionX()
		{ return x; }

		public float GetPositionY()
		{ return y; }

		public virtual void SetPosition(Vector2 newPosition)
		{
			bool Recalucate = (x != newPosition.X || y!= newPosition.Y) ;

			x = newPosition.X; y = newPosition.Y; 
			
			if(Recalucate)
			{
                onPositionChanged?.Invoke(new Vector2(x, y)); onTransformChanged?.Invoke();
            }
	
		}

		public virtual void SetPosition(float newX, float newY)
		{
            bool Recalucate = (x != newX || y != newY);

            x = newX; y = newY;

            if (Recalucate)
            {
                onPositionChanged?.Invoke(new Vector2(x, y)); onTransformChanged?.Invoke();
            }
        }

		public virtual void SetPositionX(float newX)
		{
			bool Recalucate = (x != newX);
			x = newX;
			if (Recalucate)
			{
				onPositionChanged?.Invoke(new Vector2(x, y)); onTransformChanged?.Invoke();
			}

		}
		public virtual void SetPositionY(float newY)
		{
            bool Recalucate = (y != newY);

            y = newY;


            if (Recalucate)
            {
                onPositionChanged?.Invoke(new Vector2(x, y)); onTransformChanged?.Invoke();
            }
        }


		public float GetRotationInDeg()
		{ return rotation; }

		public float GetRotationInRad()
		{ return rotation * (MathF.PI / 180); }

		public Vector2 GetForwardVector()
		{ return forward; }

		public Vector2 GetUpVector()
		{ return up; }


		// Adds rotation to the current rotation
		public void Rotate(float angleInDeg)
		{
			angleInDeg = angleInDeg % 360;

			rotation += angleInDeg;

			forward = Utils.MathUtils.RotateVector2InDeg(forward, angleInDeg);
			up = Utils.MathUtils.RotateVector2InDeg(up, angleInDeg);
			/*
			Console.WriteLine("Rotation" + Rotation);

            Console.WriteLine("forward " + forward);
			
            Console.WriteLine("up " + up);
			*/



			// event needs to be at the bottom
			onRotationChanged?.Invoke(rotation);
			onTransformChanged?.Invoke();
		}

		// Rotates towards the given angle
		public void SetRotation(float angle)
		{
			angle = angle % 360;

			float angleInRadiants = (angle - 90) * (MathF.PI / 180);

			rotation = angle;

			forward.X = -MathF.Sin(angleInRadiants);
			forward.Y = MathF.Cos(angleInRadiants);

			up = Utils.MathUtils.RotateVector2InDeg(forward, 90);

			/*
			Console.WriteLine("forward " + forward);

			Console.WriteLine("up " + up);
			*/

			// event needs to be at the bottom
			onRotationChanged?.Invoke(rotation);
			onTransformChanged?.Invoke();
		}

		public void RotateTowards(float x, float y)
		{
			//////////////// Code von copilot zum abbilden eines einheits Vector 2s auf 0-360  

			// -y wegen des komischen koordinaten systems
			float angleRad = (float)Math.Atan2(x, -y); // Ergebnis in Radiant (-π bis π)
			float angleDeg = angleRad * (180f / (float)Math.PI); // Umrechnung in Grad

			// Falls negativ, auf positiven Bereich abbilden
			if (angleDeg < 0)
				angleDeg += 360f;

			SetRotation(angleDeg - 90);

			//////////// 
		}

		public void RotateTowards(Vector2 direction)
		{
			RotateTowards(direction.X, direction.Y);
		}

		public override void Destroy()
		{
			base.Destroy();

			onDestroyed?.Invoke(this);

		}

		/// <summary>
		/// Gets Called when a GameObjectComponent has been added to this GameObject
		/// </summary>
		public virtual void ComponentAdded(InstantiableComponent component)
		{
			onComponentAdded?.Invoke(component);

			Components.Add(component);
		}

		/// <summary>
		/// Gets Called when a GameObjectComponents Destroyed Method has been called.
		/// Use this to manually remove references
		/// </summary>
		public virtual void ComponentDestroyRequested(InstantiableComponent component)
		{
			onComponentDestroyRequested?.Invoke(component);

			try // using reflection to look for the references of the component marked for deletion in the fields and propertys and setting them null 
			{
				Type type = this.GetType();
				var variables = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

				foreach (var variable in variables)
				{
					var Variablevalue = variable.GetValue(this); // getting the value of the varaible

					if (ReferenceEquals(Variablevalue, component)) // chcking if the value of the found variable is the same as the component marked for delte
					{
                        Console.WriteLine(variable.Name + "maked for deletion");
						variable.SetValue(this, null);
					}
				}

				var propertys = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

				foreach (var property in propertys)
				{
					var propertyValue = property.GetValue(this);

					if (ReferenceEquals(propertyValue, component))
					{
						Console.WriteLine(property.Name + "maked for deletion");
						property.SetValue(this, null);
					}
				}
				Components.Remove(component);
			}
			catch
			{
				throw new ArgumentException("cant null it cause the type found was worng you tried to delete something other then a compoent");
			}
			
		}

	}
}
