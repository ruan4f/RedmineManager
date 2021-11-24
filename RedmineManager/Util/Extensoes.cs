using System;
using System.Collections.Generic;
using System.Text;

namespace RedmineManager.Util
{
    public static class Extensoes
    {
        /// <summary>
        /// Converte uma string em um número, caso a conversão seja igual a zero, o método retornará o valor informado (default)
        /// </summary>
        /// <param name="numeroInString">string</param>
        /// <returns>int</returns>
        public static int ParaInteiroComDefault(this string numeroInString, int defaultValue)
        {
            var numero = 0;

            Int32.TryParse(numeroInString, out numero);

            if (numero == 0)
                return defaultValue;

            return numero;
        }

        /// <summary>
        /// Converte uma string em um número
        /// </summary>
        /// <param name="numeroInString">string</param>
        /// <returns>int</returns>
        public static int ParaInteiro(this string numeroInString)
        {
            var numero = 0;

            Int32.TryParse(numeroInString, out numero);

            return numero;
        }

        /// <summary>
        /// Converte uma string em um número
        /// </summary>
        /// <param name="numeroInString">string</param>
        /// <returns>long</returns>
        public static long ParaInteiroLongo(this string numeroInString)
        {
            long numero = 0;

            Int64.TryParse(numeroInString, out numero);

            return numero;
        }

        /// <summary>
        /// Converte uma string em um número, caso a conversão seja igual a zero, o método retornará o valor informado (default)
        /// </summary>
        /// <param name="numeroInString">string</param>
        /// <returns>int</returns>
        public static long ParaInteiroLongoComDefault(this string numeroInString, long defaultValue)
        {
            long numero = 0;

            long.TryParse(numeroInString, out numero);

            if (numero == 0)
                return defaultValue;

            return numero;
        }

        /// <summary>
        /// Converte uma string em uma data
        /// </summary>
        /// <param name="numeroInString">string</param>
        /// <returns>int</returns>
        public static DateTime ParaData(this string numeroInString)
        {
            DateTime data = DateTime.MaxValue;

            DateTime.TryParse(numeroInString, out data);

            return data;
        }

        /// <summary>
        /// Verifica se o valor foi informado
        /// </summary>
        /// <param name="numeroInString">string</param>
        /// <returns>bool</returns>
        public static bool FoiInformado(this string numeroInString)
        {
            return !string.IsNullOrWhiteSpace(numeroInString);
        }

        /// <summary>
        /// Verifica se o valor foi informado
        /// </summary>
        /// <param name="numero">numero</param>
        /// <returns>bool</returns>
        public static bool FoiInformado(this int numero)
        {
            return numero > 0;
        }
    }
}
