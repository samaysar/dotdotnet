using Dot.Net.DevFast.Collections.Interfaces;
using Dot.Net.DevFast.Etc;
using Dot.Net.DevFast.Extensions;

namespace Dot.Net.DevFast.Collections
{
    /// <inheritdoc />
    /// <summary>
    /// Simply throws exception saying heap initially created supposed to have fixed capacity.
    /// </summary>
    public sealed class HeapNoResizing : IResizeStrategy
    {
        /// <summary>
        /// Calling this method will always returns false.
        /// </summary>
        /// <param name="currentSize">Current size of the heap</param>
        /// <param name="newSize">Always outs Default int value</param>
        public bool TryComputeNewSize(int currentSize, out int newSize)
        {
            newSize = 0;
            return false;
        }

        /// <inheritdoc />
        public bool CanResize => false;
    }

    /// <inheritdoc />
    /// <summary>
    /// Increases size of the heap in a fixed size steps.
    /// </summary>
    public sealed class StepHeapResizing : IResizeStrategy
    {
        private readonly long _stepSize;

        /// <summary>
        /// Ctor with step size.
        /// </summary>
        /// <param name="stepSize">Step size for the increments.</param>
        /// <exception cref="DdnDfException">When step size is zero (0) or negative.</exception>
        public StepHeapResizing(int stepSize)
        {
            _stepSize = stepSize.ThrowIfLess(1, $"{nameof(stepSize)} cannot be zero (0) or negative.");
        }

        /// <summary>
        /// New size is simply the sum of the initial fixed step size and current capacity.
        /// Returns false in case of overflow.
        /// </summary>
        /// <param name="currentSize">Current size of the heap</param>
        /// <param name="newSize">outs new size</param>
        public bool TryComputeNewSize(int currentSize, out int newSize)
        {
            newSize = currentSize;
            var newVal = currentSize + _stepSize;
            if (newVal > int.MaxValue) return false;
            newSize = (int) newVal;
            return true;
        }

        /// <inheritdoc />
        public bool CanResize => true;
    }

    /// <inheritdoc />
    /// <summary>
    /// Increases size of the heap by given percentage.
    /// </summary>
    public sealed class PercentHeapResizing : IResizeStrategy
    {
        private readonly double _multiplier;

        /// <summary>
        /// Ctor with increment percentage.
        /// <para>
        /// Value 100 means new value will be increased by 100 percent (i.e. double).
        /// Similarly 50 means value will be multiplied by 1.5 (cast to int).
        /// </para>
        /// <para>
        /// IMPORTANT: If the percent increase (after int cast) yield no increase, then value
        /// will be increased by 1.
        /// </para>
        /// </summary>
        /// <param name="incrementPercentage">Percentage to use.</param>
        /// <exception cref="DdnDfException">When given percentage is zero (0) or negative.</exception>
        public PercentHeapResizing(int incrementPercentage)
        {
            incrementPercentage.ThrowIfLess(1, $"{nameof(incrementPercentage)} cannot be zero (0) or negative.");
            _multiplier = 1 + incrementPercentage / 100.0;
        }

        /// <summary>
        /// New size is multiplier increased (int cast), with lower bound to <paramref name="currentSize"/>+1.
        /// Returns false in case of overflow.
        /// </summary>
        /// <param name="currentSize">Current size of the heap</param>
        /// <param name="newSize">outs new size</param>
        public bool TryComputeNewSize(int currentSize, out int newSize)
        {
            newSize = currentSize;
            var newVal = (long) (currentSize * _multiplier);
            if (newVal > int.MaxValue) return false;
            newSize = newVal.Equals(currentSize) ? currentSize + 1 : (int) newVal;
            return true;
        }

        /// <inheritdoc />
        public bool CanResize => true;
    }
}