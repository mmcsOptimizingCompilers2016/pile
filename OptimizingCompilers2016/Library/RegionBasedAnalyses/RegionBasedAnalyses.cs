using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using OptimizingCompilers2016.Library.RegionBasedAnalyses;
using OptimizingCompilers2016.Library.Semilattice;

namespace OptimizingCompilers2016.Library
{


    public abstract class RegionBasedAnalyses<T> : Semilattice<T>
        where T : ICloneable
    {
        protected Dictionary<Region, T> outs = new Dictionary<Region, T>();
        protected Dictionary<Region, T> ins = new Dictionary<Region, T>();
        protected abstract T Transfer(T x, Region b);
        public abstract T Collect(T x, T y);
        protected abstract T SetStartingSet();


        void RegionBasedAlgorithm(ControlFlowGraph CFG) {

            // 1 шаг алгоритма : находим восходящую  последовательность

            var RHierarchy = new RegionHierarchy(CFG);

            var Hierarchy = RHierarchy.Hierarchy;


            //  инициализируем начальными элементами

            foreach (var Region in Hierarchy)
            {
                outs.Add(Region, SetStartingSet());
                ins.Add(Region, SetStartingSet());
            }


            // 2 шаг: восходящий анализ для вычисления передаточных функций

            foreach (var Region in Hierarchy) {


                // если область - базовый блок
                if (Region.GetType() == typeof(BaseBlockRegion))
                {
                    //ins[Region] = I - тожд функция
                    outs[Region] = Transfer(ins[Region], Region);
                } // область тела
                else if (Region.GetType() == typeof(CycleBodyRegion)) {

                    // рассматриваем подобласти
                    // TODO: подобласти должны быть отсортированы в топологическом порядке  
                    foreach (var SubRegion in Region.HierarchyLevel.Vertices) {

                        // нужно найти все подобласти в Region, у которых есть дуги, ведущие в SubRegion
                        // 

                        var PredSubRegion = Region.HierarchyLevel.Edges.ToList<Edge<Region>>()
                               .Where(edge => edge.Target.Equals(SubRegion))
                               .Select(edge => edge.Source).ToList<Region>();

                       

                        if (PredSubRegion.Any<Region>()) // иначе подобласть - заголовок области
                        {

                            T CollectRes = SetStartingSet();

                            foreach (var R in PredSubRegion ) {

                                CollectRes = Collect(CollectRes, outs[R]);

                            }

                            // для каждого выходного блока из SubRegion

                        }
                       




                    }


                }



            }


        

       }

   

    }
}
