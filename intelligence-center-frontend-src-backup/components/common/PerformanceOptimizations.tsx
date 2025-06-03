import React, { memo, useMemo, useCallback } from 'react';
import styled, { keyframes } from 'styled-components';
import { colors } from '../styled/StyledComponents';

// Performance optimized loading spinner with hardware acceleration
const spinAnimation = keyframes`
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
`;

export const OptimizedSpinner = styled.div`
  width: 40px;
  height: 40px;
  border: 3px solid ${colors.borderLight};
  border-top: 3px solid ${colors.primary};
  border-radius: 50%;
  animation: ${spinAnimation} 1s linear infinite;
  will-change: transform;
  transform: translateZ(0);
  margin: 20px auto;
`;

// Intersection Observer hook for lazy loading
export const useIntersectionObserver = (
  ref: React.RefObject<Element>,
  options: IntersectionObserverInit = {}
) => {
  const [isIntersecting, setIsIntersecting] = React.useState(false);

  React.useEffect(() => {
    const element = ref.current;
    if (!element) return;

    const observer = new IntersectionObserver(
      ([entry]) => {
        setIsIntersecting(entry.isIntersecting);
      },
      { threshold: 0.1, ...options }
    );

    observer.observe(element);
    return () => observer.disconnect();
  }, [ref, options]);

  return isIntersecting;
};

// Performance optimized lazy load wrapper
interface LazyLoadWrapperProps {
  children: React.ReactNode;
  height?: string;
  placeholder?: React.ReactNode;
}

export const LazyLoadWrapper: React.FC<LazyLoadWrapperProps> = memo(({
  children,
  height = '400px',
  placeholder
}) => {
  const ref = React.useRef<HTMLDivElement>(null);
  const isVisible = useIntersectionObserver(ref as React.RefObject<Element>, { threshold: 0.1 });

  return (
    <div ref={ref} style={{ minHeight: height }}>
      {isVisible ? children : (placeholder || <OptimizedSpinner />)}
    </div>
  );
});

LazyLoadWrapper.displayName = 'LazyLoadWrapper';

// Debounced callback hook
export const useDebounce = <T extends (...args: any[]) => any>(
  callback: T,
  delay: number
): T => {
  const timeoutRef = React.useRef<NodeJS.Timeout | null>(null);

  const debouncedCallback = useCallback((...args: Parameters<T>) => {
    if (timeoutRef.current) {
      clearTimeout(timeoutRef.current);
    }
    timeoutRef.current = setTimeout(() => {
      callback(...args);
    }, delay);
  }, [callback, delay]);

  return debouncedCallback as T;
};

// Throttled callback hook for scroll events
export const useThrottle = <T extends (...args: any[]) => any>(
  callback: T,
  delay: number
): T => {
  const lastRunRef = React.useRef<number>(0);

  const throttledCallback = useCallback((...args: Parameters<T>) => {
    const now = Date.now();
    if (now - lastRunRef.current >= delay) {
      lastRunRef.current = now;
      callback(...args);
    }
  }, [callback, delay]);

  return throttledCallback as T;
};

// Memoized data processor
export const useMemoizedData = <T, R>(
  data: T,
  processor: (data: T) => R
): R => {
  return useMemo(() => {
    if (!data) return null as R;
    return processor(data);
  }, [data, processor]);
};

const PerformanceOptimizations = {
  OptimizedSpinner,
  LazyLoadWrapper,
  useIntersectionObserver,
  useDebounce,
  useThrottle,
  useMemoizedData
};

export default PerformanceOptimizations;
