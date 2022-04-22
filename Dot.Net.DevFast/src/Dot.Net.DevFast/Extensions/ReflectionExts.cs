using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Dot.Net.DevFast.Extensions
{
    /// <summary>
    /// Extension methods related to reflection.
    /// </summary>
    public static class ReflectionExts
    {
        /// <summary>
        /// Finds Generic method using <paramref name="methodName"/> inside <paramref name="declaringType"/>,
        /// then, invokes it using given <paramref name="classInstance"/>
        /// and <paramref name="methodParameters"/> and finally awaits on the invocation.
        /// See also <seealso cref="InvokeNonValueMethodAsync(MethodInfo, object[], object)"/>.
        /// <para>
        /// Method MUST return either <see langword="void"/> or a <see cref="Task"/>.
        /// </para>
        /// </summary>
        /// <param name="declaringType">Type that contains the method</param>
        /// <param name="methodName">Name of the method. HINT: Use <see langword="nameof"/> instead
        /// of passing hard-coded string to avoid any refactoring changes.</param>
        /// <param name="methodGenerics">Generic types to apply on the method; if any.</param>
        /// <param name="methodParameters">Parameters to pass to method; if any.</param>
        /// <param name="classInstance">Instance of the class (if method is not static).
        /// For static methods keep this value <see langword="null"/></param>
        /// <param name="bindingFlags">Binding flags to use to find the correct method.</param>
        /// <exception cref="ArgumentException">When method return type cannot be assigned to <see cref="Task"/>
        /// nor to <see langword="void"/></exception>
        public static async Task InvokeNonValueMethodAsync(this Type declaringType,
            string methodName,
            Type[] methodGenerics = null,
            object[] methodParameters = null,
            object classInstance = null,
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static)
        {
            await declaringType
                .GetMethodInfo(methodName, bindingFlags, methodGenerics)
                .InvokeNonValueMethodAsync(methodParameters, classInstance)
                .ConfigureAwait(false);

        }

        /// <summary>
        /// Invokes the provided <paramref name="method"/> with given <paramref name="classInstance"/>
        /// and <paramref name="methodParameters"/> and awaits on the invocation.
        /// <para>
        /// Method MUST return either <see langword="void"/> or a <see cref="Task"/>.
        /// </para>
        /// </summary>
        /// <param name="method">Method to invoke.</param>
        /// <param name="methodParameters">Parameters to pass to method; if any.</param>
        /// <param name="classInstance">Instance of the class (if method is not static).
        /// For static methods keep this value <see langword="null"/></param>
        /// <exception cref="ArgumentException">When method return type cannot be assigned to <see cref="Task"/>
        /// nor to <see langword="void"/></exception>
        public static async Task InvokeNonValueMethodAsync(this MethodInfo method,
            object[] methodParameters = null,
            object classInstance = null)
        {
            if (typeof(Task) == method.ReturnType)
            {
                var task = (Task)method.Invoke(classInstance, methodParameters);
                await task.ConfigureAwait(false);
                return;
            }

            if (typeof(void).IsAssignableFrom(method.ReturnType))
            {
                method.Invoke(classInstance, methodParameters);
                return;
            }

            throw new ArgumentException($"Method (Name={method.Name}, Inside={method.DeclaringType?.FullName})" +
                                        $" returns '{method.ReturnType.FullName}' which cannot be" +
                                        $" assigned to {typeof(Task)} neither to {typeof(void)}");
        }

        /// <summary>
        /// Finds generic method using <paramref name="methodName"/> inside <paramref name="declaringType"/>,
        /// then, invokes it using given <paramref name="classInstance"/>
        /// and <paramref name="methodParameters"/> and finally awaits on the invocation.
        /// See also <seealso cref="InvokeValueMethodAsync{TResult}(MethodInfo, object[], object)"/>.
        /// <para>
        /// Method MUST return either a <see cref="Task{TResult}"/> OR
        /// an instance of <typeparamref name="TResult"/> OR
        /// a derived type of <typeparamref name="TResult"/> (derived type with or without Task).
        /// </para>
        /// </summary>
        /// <typeparam name="TResult">Method return type.</typeparam>
        /// <param name="declaringType">Type that contains the method</param>
        /// <param name="methodName">Name of the method. HINT: Use <see langword="nameof"/> instead
        /// of passing hard-coded string to avoid any refactoring changes.</param>
        /// <param name="methodGenerics">Generic types to apply on the method; if any.</param>
        /// <param name="methodParameters">Parameters to pass to method; if any.</param>
        /// <param name="classInstance">Instance of the class (if method is not static).
        /// For static methods keep this value <see langword="null"/></param>
        /// <param name="bindingFlags">Binding flags to use to find the correct method.</param>
        /// <exception cref="ArgumentException">When method return type cannot be assigned to <see cref="Task{TResult}"/>
        /// nor to <typeparamref name="TResult"/></exception>
        public static async Task<TResult> InvokeValueMethodAsync<TResult>(this Type declaringType,
            string methodName,
            Type[] methodGenerics = null,
            object[] methodParameters = null,
            object classInstance = null,
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static)
        {
            return await declaringType
                .GetMethodInfo(methodName, bindingFlags, methodGenerics)
                .InvokeValueMethodAsync<TResult>(methodParameters, classInstance)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Invokes the provided <paramref name="method"/> with given <paramref name="classInstance"/>
        /// and <paramref name="methodParameters"/> and awaits on the invocation.
        /// <para>
        /// Method MUST return either a <see cref="Task{TResult}"/> OR
        /// an instance of <typeparamref name="TResult"/> OR
        /// a derived type of <typeparamref name="TResult"/> (derived type with or without Task).
        /// </para>
        /// </summary>
        /// <param name="method">Method to invoke.</param>
        /// <param name="methodParameters">Parameters to pass to method; if any.</param>
        /// <param name="classInstance">Instance of the class (if method is not static).
        /// For static methods keep this value <see langword="null"/></param>
        /// <exception cref="ArgumentException">When method return type cannot be assigned to <see cref="Task{TResult}"/>
        /// nor to <typeparamref name="TResult"/></exception>
        public static async Task<TResult> InvokeValueMethodAsync<TResult>(this MethodInfo method,
            object[] methodParameters = null,
            object classInstance = null)
        {
            if (typeof(Task<TResult>).IsAssignableFrom(method.ReturnType))
            {
                var task = (Task<TResult>)method.Invoke(classInstance, methodParameters);
                return await task.ConfigureAwait(false);
            }

            if (method.ReturnType.IsGenericType &&
                method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>) &&
                typeof(TResult).IsAssignableFrom(method.ReturnType.GetGenericArguments()[0]))
            {
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
                return await typeof(ReflectionExts).InvokeValueMethodAsync<TResult>(nameof(InvokeResultAsync),
                    new[] { typeof(TResult), method.ReturnType.GetGenericArguments()[0] },
                    new[] { method, classInstance, methodParameters },
                    null,
                    BindingFlags.Static | BindingFlags.NonPublic).ConfigureAwait(false);
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
            }

            if (typeof(TResult).IsAssignableFrom(method.ReturnType))
            {
                return (TResult)method.Invoke(classInstance, methodParameters);
            }

            throw new ArgumentException($"Method (Name={method.Name}, Inside={method.DeclaringType?.FullName})" +
                                        $" returns '{method.ReturnType.FullName}' which cannot be" +
                                        $" assigned to {typeof(Task<TResult>)} neither to {typeof(TResult)}");
        }

        private static async Task<TOut> InvokeResultAsync<TOut, TIn>(MethodBase method, 
            object instance,
            object[] parameters)
            where TIn : TOut
        {
            // ReSharper disable once PossibleNullReferenceException
            return await ((Task<TIn>)method.Invoke(instance, parameters)).ConfigureAwait(false);
        }

        private static MethodInfo GetMethodInfo(this Type methodClass,
            string methodName,
            BindingFlags bindingFlags,
            Type[] methodGenerics = null)
        {
            var method = methodClass.GetMethod(methodName, bindingFlags) ??
                         throw new MissingMethodException($"Method (Name={methodName})" +
                                                          $" not found inside {methodClass.FullName} with flags={bindingFlags:F}");
            return methodGenerics == null ? method : method.MakeGenericMethod(methodGenerics);
        }
    }
}