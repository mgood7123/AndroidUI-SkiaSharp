using System;
using System.ComponentModel;

namespace SkiaSharp
{
	public unsafe partial struct SKMatrix
	{
		internal const float DegreesToRadians = (float)Math.PI / 180.0f;

		public readonly static SKMatrix Empty;

		public readonly static SKMatrix Identity = new SKMatrix { scaleX = 1, scaleY = 1, persp2 = 1 };

		public SKMatrix (float[] values)
		{
			if (values == null)
				throw new ArgumentNullException (nameof (values));
			if (values.Length != 9)
				throw new ArgumentException ($"The matrix array must have a length of 9.", nameof (values));

			scaleX = values[(int)SKMatrixRowMajorMask.ScaleX];
			skewX = values[(int)SKMatrixRowMajorMask.SkewX];
			transX = values[(int)SKMatrixRowMajorMask.TransX];

			skewY = values[(int)SKMatrixRowMajorMask.SkewY];
			scaleY = values[(int)SKMatrixRowMajorMask.ScaleY];
			transY = values[(int)SKMatrixRowMajorMask.TransY];

			persp0 = values[(int)SKMatrixRowMajorMask.Persp0];
			persp1 = values[(int)SKMatrixRowMajorMask.Persp1];
			persp2 = values[(int)SKMatrixRowMajorMask.Persp2];
		}

		public SKMatrix (
			float scaleX, float skewX, float transX,
			float skewY, float scaleY, float transY,
			float persp0, float persp1, float persp2)
		{
			this.scaleX = scaleX;
			this.skewX = skewX;
			this.transX = transX;
			this.skewY = skewY;
			this.scaleY = scaleY;
			this.transY = transY;
			this.persp0 = persp0;
			this.persp1 = persp1;
			this.persp2 = persp2;
		}

		private void setFrom (ref SKMatrix m)
		{
			scaleX = m.scaleX;
			skewX = m.skewX;
			transX = m.transX;
			skewY = m.skewY;
			scaleY = m.scaleY;
			transY = m.transY;
			persp0 = m.persp0;
			persp1 = m.persp1;
			persp2 = m.persp2;
		}

		public readonly bool IsIdentity {
			get {
				fixed (SKMatrix* t = &this) {
					return SkiaApi.sk_matrix_is_identity (t);
				};
			}
		}

		public readonly bool IsTranslate {
			get {
				fixed (SKMatrix* t = &this) {
					return SkiaApi.sk_matrix_is_translate (t);
				};
			}
		}

		public readonly bool IsScaleTranslate {
			get {
				fixed (SKMatrix* t = &this) {
					return SkiaApi.sk_matrix_is_scale_translate (t);
				};
			}
		}

		public bool IsSimilarity(float tol) {
			fixed (SKMatrix* t = &this) {
				return SkiaApi.sk_matrix_is_similarity (t, tol);
			};
		}

		// Values

		public float get(SKMatrixRowMajorMask index)
		{
			fixed (SKMatrix* t = &this) {
				return SkiaApi.sk_matrix_get (t, index);
			}
		}

		public float[] Values {
			readonly get =>
				new float[9] {
					scaleX, skewX, transX,
					skewY, scaleY, transY,
					persp0, persp1, persp2
				};
			set {
				if (value == null)
					throw new ArgumentNullException (nameof (Values));
				if (value.Length != 9)
					throw new ArgumentException ($"The matrix array must have a length of 9.", nameof (Values));

				scaleX = value[(int)SKMatrixRowMajorMask.ScaleX];
				skewX = value[(int)SKMatrixRowMajorMask.SkewX];
				transX = value[(int)SKMatrixRowMajorMask.TransX];

				skewY = value[(int)SKMatrixRowMajorMask.SkewY];
				scaleY = value[(int)SKMatrixRowMajorMask.ScaleY];
				transY = value[(int)SKMatrixRowMajorMask.TransY];

				persp0 = value[(int)SKMatrixRowMajorMask.Persp0];
				persp1 = value[(int)SKMatrixRowMajorMask.Persp1];
				persp2 = value[(int)SKMatrixRowMajorMask.Persp2];
			}
		}

		public readonly void GetValues (float[] values)
		{
			if (values == null)
				throw new ArgumentNullException (nameof (values));
			if (values.Length != 9)
				throw new ArgumentException ($"The matrix array must have a length of 9.", nameof (values));

			values[(int)SKMatrixRowMajorMask.ScaleX] = scaleX;
			values[(int)SKMatrixRowMajorMask.SkewX] = skewX;
			values[(int)SKMatrixRowMajorMask.TransX] = transX;

			values[(int)SKMatrixRowMajorMask.SkewY] = skewY;
			values[(int)SKMatrixRowMajorMask.ScaleY] = scaleY;
			values[(int)SKMatrixRowMajorMask.TransY] = transY;

			values[(int)SKMatrixRowMajorMask.Persp0] = persp0;
			values[(int)SKMatrixRowMajorMask.Persp1] = persp1;
			values[(int)SKMatrixRowMajorMask.Persp2] = persp2;
		}

		// Create*

		public static SKMatrix CreateIdentity () =>
			new SKMatrix { scaleX = 1, scaleY = 1, persp2 = 1 };

		public static SKMatrix CreateTranslation (float x, float y)
		{
			if (x == 0 && y == 0)
				return Identity;

			return new SKMatrix {
				scaleX = 1,
				scaleY = 1,
				transX = x,
				transY = y,
				persp2 = 1,
			};
		}

		public static SKMatrix CreateScale (float x, float y)
		{
			if (x == 1 && y == 1)
				return Identity;

			return new SKMatrix {
				scaleX = x,
				scaleY = y,
				persp2 = 1,
			};
		}

		public static SKMatrix CreateScale (float x, float y, float pivotX, float pivotY)
		{
			if (x == 1 && y == 1)
				return Identity;

			var tx = pivotX - x * pivotX;
			var ty = pivotY - y * pivotY;

			return new SKMatrix {
				scaleX = x,
				scaleY = y,
				transX = tx,
				transY = ty,
				persp2 = 1,
			};
		}

		public static SKMatrix CreateRotation (float radians)
		{
			if (radians == 0)
				return Identity;

			var sin = (float)Math.Sin (radians);
			var cos = (float)Math.Cos (radians);

			var matrix = Identity;
			SetSinCos (ref matrix, sin, cos);
			return matrix;
		}

		public static SKMatrix CreateRotation (float radians, float pivotX, float pivotY)
		{
			if (radians == 0)
				return Identity;

			var sin = (float)Math.Sin (radians);
			var cos = (float)Math.Cos (radians);

			var matrix = Identity;
			SetSinCos (ref matrix, sin, cos, pivotX, pivotY);
			return matrix;
		}

		public static SKMatrix CreateRotationDegrees (float degrees)
		{
			if (degrees == 0)
				return Identity;

			return CreateRotation (degrees * DegreesToRadians);
		}

		public static SKMatrix CreateRotationDegrees (float degrees, float pivotX, float pivotY)
		{
			if (degrees == 0)
				return Identity;

			return CreateRotation (degrees * DegreesToRadians, pivotX, pivotY);
		}

		public static SKMatrix CreateSkew (float x, float y)
		{
			if (x == 0 && y == 0)
				return Identity;

			return new SKMatrix {
				scaleX = 1,
				skewX = x,
				skewY = y,
				scaleY = 1,
				persp2 = 1,
			};
		}

		public static SKMatrix CreateScaleTranslation (float sx, float sy, float tx, float ty)
		{
			if (sx == 0 && sy == 0 && tx == 0 && ty == 0)
				return Identity;

			return new SKMatrix {
				scaleX = sx,
				skewX = 0,
				transX = tx,

				skewY = 0,
				scaleY = sy,
				transY = ty,

				persp0 = 0,
				persp1 = 0,
				persp2 = 1,
			};
		}

		// Make*

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use CreateIdentity() instead.")]
		public static SKMatrix MakeIdentity () =>
			CreateIdentity ();

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use CreateScale(float, float) instead.")]
		public static SKMatrix MakeScale (float sx, float sy) =>
			CreateScale (sx, sy);

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use CreateScale(float, float, float, float) instead.")]
		public static SKMatrix MakeScale (float sx, float sy, float pivotX, float pivotY) =>
			CreateScale (sx, sy, pivotX, pivotY);

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use CreateTranslation(float, float) instead.")]
		public static SKMatrix MakeTranslation (float dx, float dy) =>
			CreateTranslation (dx, dy);

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use CreateRotation(float) instead.")]
		public static SKMatrix MakeRotation (float radians) =>
			CreateRotation (radians);

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use CreateRotation(float, float, float) instead.")]
		public static SKMatrix MakeRotation (float radians, float pivotx, float pivoty) =>
			CreateRotation (radians, pivotx, pivoty);

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use CreateRotationDegrees(float) instead.")]
		public static SKMatrix MakeRotationDegrees (float degrees) =>
			CreateRotationDegrees (degrees);

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use CreateRotationDegrees(float, float, float) instead.")]
		public static SKMatrix MakeRotationDegrees (float degrees, float pivotx, float pivoty) =>
			CreateRotationDegrees (degrees, pivotx, pivoty);

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use CreateSkew(float, float) instead.")]
		public static SKMatrix MakeSkew (float sx, float sy) =>
			CreateSkew (sx, sy);

		// Set*

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use CreateScaleTranslation(float, float, float, float) instead.")]
		public void SetScaleTranslate (float sx, float sy, float tx, float ty)
		{
			scaleX = sx;
			skewX = 0;
			transX = tx;

			skewY = 0;
			scaleY = sy;
			transY = ty;

			persp0 = 0;
			persp1 = 0;
			persp2 = 1;
		}

		// Rotate

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use CreateRotation(float, float, float) instead.")]
		public static void Rotate (ref SKMatrix matrix, float radians, float pivotx, float pivoty)
		{
			var sin = (float)Math.Sin (radians);
			var cos = (float)Math.Cos (radians);
			SetSinCos (ref matrix, sin, cos, pivotx, pivoty);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use CreateRotationDegrees(float, float, float) instead.")]
		public static void RotateDegrees (ref SKMatrix matrix, float degrees, float pivotx, float pivoty)
		{
			var sin = (float)Math.Sin (degrees * DegreesToRadians);
			var cos = (float)Math.Cos (degrees * DegreesToRadians);
			SetSinCos (ref matrix, sin, cos, pivotx, pivoty);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use CreateRotation(float) instead.")]
		public static void Rotate (ref SKMatrix matrix, float radians)
		{
			var sin = (float)Math.Sin (radians);
			var cos = (float)Math.Cos (radians);
			SetSinCos (ref matrix, sin, cos);
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use CreateRotationDegrees(float) instead.")]
		public static void RotateDegrees (ref SKMatrix matrix, float degrees)
		{
			var sin = (float)Math.Sin (degrees * DegreesToRadians);
			var cos = (float)Math.Cos (degrees * DegreesToRadians);
			SetSinCos (ref matrix, sin, cos);
		}

		// Invert

		public readonly bool IsInvertible {
			get {
				fixed (SKMatrix* t = &this) {
					return SkiaApi.sk_matrix_try_invert (t, null);
				}
			}
		}

		public readonly bool TryInvert (out SKMatrix inverse)
		{
			fixed (SKMatrix* i = &inverse)
			fixed (SKMatrix* t = &this) {
				return SkiaApi.sk_matrix_try_invert (t, i);
			}
		}

		public readonly SKMatrix Invert ()
		{
			if (TryInvert (out var matrix))
				return matrix;

			return Empty;
		}

		// *Concat

		public static SKMatrix Concat (SKMatrix first, SKMatrix second)
		{
			SKMatrix target;
			SkiaApi.sk_matrix_concat (&target, &first, &second);
			return target;
		}

		public readonly SKMatrix PreConcat (SKMatrix matrix)
		{
			var target = this;
			SkiaApi.sk_matrix_pre_concat (&target, &matrix);
			return target;
		}

		public readonly SKMatrix PostConcat (SKMatrix matrix)
		{
			var target = this;
			SkiaApi.sk_matrix_post_concat (&target, &matrix);
			return target;
		}

		public static void Concat (ref SKMatrix target, SKMatrix first, SKMatrix second)
		{
			fixed (SKMatrix* t = &target) {
				SkiaApi.sk_matrix_concat (t, &first, &second);
			}
		}

		public static void Concat (ref SKMatrix target, ref SKMatrix first, ref SKMatrix second)
		{
			fixed (SKMatrix* t = &target)
			fixed (SKMatrix* f = &first)
			fixed (SKMatrix* s = &second) {
				SkiaApi.sk_matrix_concat (t, f, s);
			}
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use PreConcat(SKMatrix) instead.")]
		public static void PreConcat (ref SKMatrix target, SKMatrix matrix)
		{
			fixed (SKMatrix* t = &target) {
				SkiaApi.sk_matrix_pre_concat (t, &matrix);
			}
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use PreConcat(SKMatrix) instead.")]
		public static void PreConcat (ref SKMatrix target, ref SKMatrix matrix)
		{
			fixed (SKMatrix* t = &target)
			fixed (SKMatrix* m = &matrix) {
				SkiaApi.sk_matrix_pre_concat (t, m);
			}
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use PostConcat(SKMatrix) instead.")]
		public static void PostConcat (ref SKMatrix target, SKMatrix matrix)
		{
			fixed (SKMatrix* t = &target) {
				SkiaApi.sk_matrix_post_concat (t, &matrix);
			}
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use PostConcat(SKMatrix) instead.")]
		public static void PostConcat (ref SKMatrix target, ref SKMatrix matrix)
		{
			fixed (SKMatrix* t = &target)
			fixed (SKMatrix* m = &matrix) {
				SkiaApi.sk_matrix_post_concat (t, m);
			}
		}

		// MapRect

		public readonly SKRect MapRect (SKRect source)
		{
			SKRect dest;
			fixed (SKMatrix* m = &this) {
				SkiaApi.sk_matrix_map_rect (m, &dest, &source);
			}
			return dest;
		}

		[EditorBrowsable (EditorBrowsableState.Never)]
		[Obsolete ("Use MapRect(SKRect) instead.")]
		public static void MapRect (ref SKMatrix matrix, out SKRect dest, ref SKRect source)
		{
			fixed (SKMatrix* m = &matrix)
			fixed (SKRect* d = &dest)
			fixed (SKRect* s = &source) {
				SkiaApi.sk_matrix_map_rect (m, d, s);
			}
		}

		// MapPoints

		public readonly SKPoint MapPoint (SKPoint point) =>
			MapPoint (point.X, point.Y);

		public readonly SKPoint MapPoint (float x, float y)
		{
			SKPoint result;
			fixed (SKMatrix* t = &this) {
				SkiaApi.sk_matrix_map_xy (t, x, y, &result);
			}
			return result;
		}

		public readonly void MapPoints (SKPoint[] result, SKPoint[] points)
		{
			if (result == null)
				throw new ArgumentNullException (nameof (result));
			if (points == null)
				throw new ArgumentNullException (nameof (points));
			if (result.Length != points.Length)
				throw new ArgumentException ("Buffers must be the same size.");

			fixed (SKMatrix* t = &this)
			fixed (SKPoint* rp = result)
			fixed (SKPoint* pp = points) {
				SkiaApi.sk_matrix_map_points (t, rp, pp, result.Length);
			}
		}

		public readonly SKPoint[] MapPoints (SKPoint[] points)
		{
			if (points == null)
				throw new ArgumentNullException (nameof (points));

			var res = new SKPoint[points.Length];
			MapPoints (res, points);
			return res;
		}

		// MapVectors

		public readonly SKPoint MapVector (SKPoint vector) =>
			MapVector (vector.X, vector.Y);

		public readonly SKPoint MapVector (float x, float y)
		{
			SKPoint result;
			fixed (SKMatrix* t = &this) {
				SkiaApi.sk_matrix_map_vector (t, x, y, &result);
			}
			return result;
		}

		public readonly void MapVectors (SKPoint[] result, SKPoint[] vectors)
		{
			if (result == null)
				throw new ArgumentNullException (nameof (result));
			if (vectors == null)
				throw new ArgumentNullException (nameof (vectors));
			if (result.Length != vectors.Length)
				throw new ArgumentException ("Buffers must be the same size.");

			fixed (SKMatrix* t = &this)
			fixed (SKPoint* rp = result)
			fixed (SKPoint* pp = vectors) {
				SkiaApi.sk_matrix_map_vectors (t, rp, pp, result.Length);
			}
		}

		public readonly SKPoint[] MapVectors (SKPoint[] vectors)
		{
			if (vectors == null)
				throw new ArgumentNullException (nameof (vectors));

			var res = new SKPoint[vectors.Length];
			MapVectors (res, vectors);
			return res;
		}

		// MapRadius

		public readonly float MapRadius (float radius)
		{
			fixed (SKMatrix* t = &this) {
				return SkiaApi.sk_matrix_map_radius (t, radius);
			}
		}

		// private

		private static void SetSinCos (ref SKMatrix matrix, float sin, float cos)
		{
			matrix.scaleX = cos;
			matrix.skewX = -sin;
			matrix.transX = 0;
			matrix.skewY = sin;
			matrix.scaleY = cos;
			matrix.transY = 0;
			matrix.persp0 = 0;
			matrix.persp1 = 0;
			matrix.persp2 = 1;
		}

		private static void SetSinCos (ref SKMatrix matrix, float sin, float cos, float pivotx, float pivoty)
		{
			float oneMinusCos = 1 - cos;

			matrix.scaleX = cos;
			matrix.skewX = -sin;
			matrix.transX = Dot (sin, pivoty, oneMinusCos, pivotx);
			matrix.skewY = sin;
			matrix.scaleY = cos;
			matrix.transY = Dot (-sin, pivotx, oneMinusCos, pivoty);
			matrix.persp0 = 0;
			matrix.persp1 = 0;
			matrix.persp2 = 1;
		}

		private static float Dot (float a, float b, float c, float d) =>
			a * b + c * d;

		private static float Cross (float a, float b, float c, float d) =>
			a * b - c * d;

		// additional
		public float Scale (float sx, float sy)
		{
			SKMatrix o;
			float r;
			fixed (SKMatrix* t = &this) {
				r = SkiaApi.sk_matrix_scale (t, sx, sy, &o);
			};
			setFrom(ref o);
			return r;
		}

		public float Translate (float dx, float dy)
		{
			SKMatrix o;
			float r;
			fixed (SKMatrix* t = &this) {
				r = SkiaApi.sk_matrix_translate (t, dx, dy, &o);
			};
			setFrom (ref o);
			return r;
		}

		public float Translate (SKPoint point)
		{
			SKMatrix o;
			float r;
			fixed (SKMatrix* t = &this) {
				r = SkiaApi.sk_matrix_translate_point (t, point, &o);
			};
			setFrom (ref o);
			return r;
		}

		public float Translate (SKPointI point)
		{
			SKMatrix o;
			float r;
			fixed (SKMatrix* t = &this) {
				r = SkiaApi.sk_matrix_translate_ipoint (t, point, &o);
			};
			setFrom (ref o);
			return r;
		}

		public float RotateDeg (float deg)
		{
			SKMatrix o;
			float r;
			fixed (SKMatrix* t = &this) {
				r = SkiaApi.sk_matrix_rotate_deg (t, deg, &o);
			};
			setFrom (ref o);
			return r;
		}

		public float RotateDeg (float deg, SKPoint pivot)
		{
			SKMatrix o;
			float r;
			fixed (SKMatrix* t = &this) {
				r = SkiaApi.sk_matrix_rotate_deg_point (t, deg, pivot, &o);
			};
			setFrom (ref o);
			return r;
		}

		public float RotateRad (float rad)
		{
			SKMatrix o;
			float r;
			fixed (SKMatrix* t = &this) {
				r = SkiaApi.sk_matrix_rotate_rad (t, rad, &o);
			};
			setFrom (ref o);
			return r;
		}

		public void SetAll (
			float scaleX, float skewX, float transX,
			float skewY, float scaleY, float transY,
			float pers0, float pers1, float pers2
		)
		{
			ScaleX = scaleX;
			SkewX = skewX;
			TransX = transX;

			ScaleY = scaleY;
			SkewY = skewY;
			TransY = transY;

			Persp0 = pers0;
			Persp1 = pers1;
			Persp2 = pers2;
		}

		public SKMatrixTypeMask Type {
			get {
				fixed (SKMatrix* t = &this) {
					return SkiaApi.sk_matrix_get_type (t);
				};
			}
		}

		public bool RectStaysRect {
			get {
				fixed (SKMatrix* t = &this) {
					return SkiaApi.sk_matrix_rect_stays_rect (t);
				};
			}
		}

		public bool PreservesAxisAlignment {
			get {
				fixed (SKMatrix* t = &this) {
					return SkiaApi.sk_matrix_preserves_axis_alignment (t);
				};
			}
		}

		public bool HasPerspective {
			get {
				fixed (SKMatrix* t = &this) {
					return SkiaApi.sk_matrix_has_perspective (t);
				};
			}
		}

		public bool PreservesRightAngles(float tol) {
			fixed (SKMatrix* t = &this) {
				return SkiaApi.sk_matrix_preserves_right_angles (t, tol);
			};
		}

		public void Set9 (float[] buffer)
		{
			if (buffer == null)
				throw new ArgumentNullException (nameof (buffer));
			if (buffer.Length != 9)
				throw new ArgumentException ($"The matrix array must have a length of 9.", nameof (buffer));

			SKMatrix o;
			fixed (float* buf = buffer)
			fixed (SKMatrix* t = &this) {
				SkiaApi.sk_matrix_set9 (t, buf, &o);
			};
			setFrom (ref o);
		}

		public void Get9 (float[] buffer)
		{
			if (buffer == null)
				throw new ArgumentNullException (nameof (buffer));
			if (buffer.Length != 9)
				throw new ArgumentException ($"The matrix array must have a length of 9.", nameof (buffer));

			fixed (float* buf = buffer)
			fixed (SKMatrix* t = &this) {
				SkiaApi.sk_matrix_get9 (t, buf);
			};
		}

		public void Reset ()
		{
			SKMatrix o;
			fixed (SKMatrix* t = &this) {
				SkiaApi.sk_matrix_reset (t, &o);
			};
			setFrom (ref o);
		}

		public void SetIdentity ()
		{
			SKMatrix o;
			fixed (SKMatrix* t = &this) {
				SkiaApi.sk_matrix_set_identity (t, &o);
			};
			setFrom (ref o);
		}

		public static void SetAffine (float[] affine)
		{
			if (affine == null)
				throw new ArgumentNullException (nameof (affine));
			if (affine.Length != 6)
				throw new ArgumentException ($"The matrix array must have a length of 6.", nameof (affine));

			affine[(int)SKMatrixAffineColomnMajorMask.AScaleX] = 1;
			affine[(int)SKMatrixAffineColomnMajorMask.ASkewY] = 0;
			affine[(int)SKMatrixAffineColomnMajorMask.ASkewX] = 0;
			affine[(int)SKMatrixAffineColomnMajorMask.AScaleY] = 1;
			affine[(int)SKMatrixAffineColomnMajorMask.ATransX] = 0;
			affine[(int)SKMatrixAffineColomnMajorMask.ATransY] = 0;
		}

		public bool AsAffine(float[] affine) {
			if (HasPerspective) {
				return false;
			}
			if (affine != null) {
				if (affine.Length != 6)
					throw new ArgumentException ($"The matrix array must have a length of 6.", nameof (affine));
				affine[(int)SKMatrixAffineColomnMajorMask.AScaleX] = ScaleX;
				affine[(int)SKMatrixAffineColomnMajorMask.ASkewY] = SkewY;
				affine[(int)SKMatrixAffineColomnMajorMask.ASkewX] = SkewX;
				affine[(int)SKMatrixAffineColomnMajorMask.AScaleY] = ScaleY;
				affine[(int)SKMatrixAffineColomnMajorMask.ATransX] = TransX;
				affine[(int)SKMatrixAffineColomnMajorMask.ATransY] = TransY;
			}
			return true;
		}

		public void setAffine(float[] buffer) {
			if (buffer == null)
				throw new ArgumentNullException (nameof (buffer));
			if (buffer.Length != 6)
				throw new ArgumentException ($"The matrix array must have a length of 6.", nameof (buffer));

			ScaleX = buffer[(int)SKMatrixAffineColomnMajorMask.AScaleX];
			SkewX  = buffer[(int)SKMatrixAffineColomnMajorMask.ASkewX];
			TransX = buffer[(int)SKMatrixAffineColomnMajorMask.ATransX];
			SkewY  = buffer[(int)SKMatrixAffineColomnMajorMask.ASkewY];
			ScaleY = buffer[(int)SKMatrixAffineColomnMajorMask.AScaleY];
			TransY = buffer[(int)SKMatrixAffineColomnMajorMask.ATransY];
			Persp0 = 0;
			Persp1 = 0;
			Persp2 = 1;
		}

		public void NormalizePerspective()
		{
			SKMatrix o;
			fixed (SKMatrix* t = &this) {
				SkiaApi.sk_matrix_normalize_perspective (t, &o);
			};
			setFrom (ref o);
		}

		public void MapHomogeneousPoints (SKPoint3[] dst, SKPoint3[] src)
		{
			MapHomogeneousPoints (dst, src, src.Length);
		}

		public void MapHomogeneousPoints(SKPoint3[] dst, SKPoint3[] src, int count)
		{
			if (dst == null)
				throw new ArgumentNullException (nameof (dst));
			if (src == null)
				throw new ArgumentNullException (nameof (src));
			if (dst.Length != src.Length)
				throw new ArgumentException ("Buffers must be the same size.");

			fixed (SKMatrix* t = &this)
			fixed (SKPoint3* rp = src)
			fixed (SKPoint3* pp = dst) {
				SkiaApi.sk_matrix_map_homogeneous_points3 (t, rp, pp, count);
			}
		}

		public void MapHomogeneousPoints (SKPoint3[] dst, SKPoint[] src)
		{
			MapHomogeneousPoints (dst, src, src.Length);
		}

		public void MapHomogeneousPoints (SKPoint3[] dst, SKPoint[] src, int count)
		{
			if (dst == null)
				throw new ArgumentNullException (nameof (dst));
			if (src == null)
				throw new ArgumentNullException (nameof (src));
			if (dst.Length != src.Length)
				throw new ArgumentException ("Buffers must be the same size.");

			fixed (SKMatrix* t = &this)
			fixed (SKPoint* rp = src)
			fixed (SKPoint3* pp = dst) {
				SkiaApi.sk_matrix_map_homogeneous_points (t, rp, pp, count);
			}
		}

		public bool IsFinite {
			get {
				fixed (SKMatrix* t = &this) {
					return SkiaApi.sk_matrix_is_finite (t);
				};
			}
		}
	}
}
